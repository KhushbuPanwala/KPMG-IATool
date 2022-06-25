using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Drawing;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using System.Collections;
using InternalAuditSystem.Repository.ApplicationClasses;

namespace InternalAuditSystem.Repository.Repository.GeneratePPTRepository
{
    public class PowerPointTemplate
    {

        public PowerPointTemplate()
        {
            this.PowerPointParameters = new List<PowerPointParameter>();
            this.SlideDictionary = new Dictionary<SlidePart, SlideId>();
        }

        public List<PowerPointParameter> PowerPointParameters { get; set; }

        public Dictionary<SlidePart, SlideId> SlideDictionary { get; set; }
        /// <summary>
        /// Convert template with data
        /// </summary>
        /// <param name="templatePath">template file path</param>
        /// <param name="templateOutputPath">output file path</param>
        public void ParseTemplate(string templatePath, string templateOutputPath)
        {
            //read our template
            using (var templateFile = File.Open(templatePath, FileMode.Open, FileAccess.Read))
            {
                using (var stream = new MemoryStream())
                {
                    templateFile.CopyTo(stream); //copy template
                    //open presentation document
                    using (var presentationDocument = PresentationDocument.Open(stream, true))
                    {
                        // Get the presentation part from the presentation document.
                        var presentationPart = presentationDocument.PresentationPart;

                        // Get the presentation from the presentation part.
                        var presentation = presentationPart.Presentation;

                        var slideList = new List<SlidePart>();

                        //get available slide list
                        foreach (SlideId slideID in presentation.SlideIdList)
                        {
                            var slide = (SlidePart)presentationPart.GetPartById(slideID.RelationshipId);
                            slideList.Add(slide);
                            SlideDictionary.Add(slide, slideID);//add to dictionary to be used when needed
                        }

                        //loop all slides and replace images and texts
                        foreach (var slide in slideList)
                        {
                            ReplaceImages(presentationDocument, slide); //replace images by name

                            var paragraphs = slide.Slide.Descendants<Paragraph>().ToList(); //get all paragraphs in the slide

                            foreach (var paragraph in paragraphs)
                            {
                                ReplaceText(paragraph); //replace text by placeholder name
                            }
                        }

                        var slideCount = presentation.SlideIdList.ToList().Count; //count slides
                        DeleteSlide(presentation, slideList[slideCount - 1]); //delete last slide

                        presentation.Save(); //save document changes we've made
                    }
                    stream.Seek(0, SeekOrigin.Begin);//scroll to stream start point

                    //save output file
                    using (var fileStream = File.Create(templateOutputPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
        }


        /// <summary>
        /// Deletes slide from presentation
        /// </summary>
        /// <param name="presentation"> Presentation </param>
        /// <param name="slidePart">part of slide</param>
        void DeleteSlide(Presentation presentation, SlidePart slidePart)
        {
            var deleletSlideI = SlideDictionary[slidePart];
            presentation.SlideIdList.RemoveChild(deleletSlideI);
        }


        /// <summary>
        /// Replaces slidePart images
        /// </summary>
        /// <param name="presentationDocument">presentation Document</param>
        /// <param name="slidePart">Part of slide</param>
        void ReplaceImages(PresentationDocument presentationDocument, SlidePart slidePart)
        {
            // get all images in the slide
            var imagesToReplace = slidePart.Slide.Descendants<Blip>().ToList();

            if (imagesToReplace.Any())
            {
                var index = 0;//image index within the slide

                //find all image names in the slide
                var slidePartImageNames = slidePart.Slide.Descendants<DocumentFormat.OpenXml.Presentation.Picture>()
                                        .Where(a => a.NonVisualPictureProperties.NonVisualDrawingProperties.Name.HasValue)
                                        .Select(a => a.NonVisualPictureProperties.NonVisualDrawingProperties.Name.Value).Distinct().ToList();

                //check all images in the slide and replace them if it matches our parameter
                foreach (var imagePlaceHolder in slidePartImageNames)
                {
                    //check if we have image parameter that matches slide part image
                    foreach (var param in PowerPointParameters)
                    {
                        //replace it if found by image name
                        if (param.Image != null && param.Name.ToLower() == imagePlaceHolder.ToLower())
                        {
                            var imagePart = slidePart.AddImagePart(ImagePartType.Jpeg); //add image to document

                            using (FileStream imgStream = new FileStream(param.Image.FullName, FileMode.Open))
                            {
                                imagePart.FeedData(imgStream); //feed it with data
                            }

                            var relId = slidePart.GetIdOfPart(imagePart); // get relationship ID

                            imagesToReplace.Skip(index).First().Embed = relId; //assign new relID, skip if this is another image in one slide part
                        }
                    }
                    index += 1;
                }
            }
        }

        /// <summary>
        /// Replace all text placeholders in paragraph
        /// </summary>
        /// <param name="paragraph">paragraph </param>
        void ReplaceText(Paragraph paragraph)
        {
            var parent = paragraph.Parent; //get parent element - to be used when removing placeholder
            var dataParam = new PowerPointParameter();

            if (ContainsParam(paragraph, ref dataParam)) //check if paragraph is on our parameter list
            {
                //insert text list
                if (dataParam.Name.Contains("string[]")) //check if param is a list
                {
                    var arrayText = dataParam.Text.Split(Environment.NewLine.ToCharArray()); //in our case we split it into lines

                    if (arrayText is IEnumerable) //enumerate if we can
                    {
                        foreach (var itemData in arrayText)
                        {
                            Paragraph bullet = CloneParaGraphWithStyles(paragraph, dataParam.Name, itemData);// create new param - preserve styles
                            parent.InsertBefore(bullet, paragraph); //insert new element
                        }
                    }
                    paragraph.Remove();//delete placeholder
                }
                else
                {
                    //insert text line
                    var param = CloneParaGraphWithStyles(paragraph, dataParam.Name, dataParam.Text); // create new param - preserve styles
                    parent.InsertBefore(param, paragraph);//insert new element

                    paragraph.Remove();//delete placeholder
                }
            }
        }

        /// <summary>
        /// Checks if process parameter to replace with text or image
        /// </summary>
        /// <param name="paragraph">paragraph</param>
        /// <returns>true or false based on paramter replace or not</returns>
        public bool ContainsParam(Paragraph paragraph, ref PowerPointParameter dataParam)
        {
            foreach (var param in this.PowerPointParameters)
            {
                if (!string.IsNullOrEmpty(param.Name) && paragraph.InnerText.ToLower().Contains(param.Name.ToLower()))
                {
                    dataParam = param;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Clones paragraph with styles
        /// </summary>
        /// <param name="sourceParagraph">source Paragraph</param>
        /// <param name="paramKey">parameter key for xml</param>
        /// <param name="text">paragraph replace text</param>
        /// <returns>Paragraph</returns>
        public static Paragraph CloneParaGraphWithStyles(Paragraph sourceParagraph, string paramKey, string text)
        {
            var xmlSource = sourceParagraph.OuterXml;
            if (text != null)
            {
                xmlSource = xmlSource.Replace(paramKey.Trim(), text.Trim());
            }
            return new Paragraph(xmlSource);
        }
    }
}
