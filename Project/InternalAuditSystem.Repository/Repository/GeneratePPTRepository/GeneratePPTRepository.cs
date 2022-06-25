using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using System.IO;
using InternalAuditSystem.Utility.Constants;
using System.Threading.Tasks;
using System.Linq;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Drawing;
using System.Reflection;
using InternalAuditSystem.Repository.ApplicationClasses;
using DocumentFormat.OpenXml;

namespace InternalAuditSystem.Repository.Repository.GeneratePPTRepository
{
    public class GeneratePPTRepository : IGeneratePPTRepository
    {

        static int index = 1;
        /// <summary>
        /// Defines id used for slide master id and slide layout id lists.
        /// </summary>
        static uint uniqueId;

        /// <summary>
        /// Desins list of dynamic dictionary key 
        /// </summary>
        public List<int> keyList { get; set; }
        public GeneratePPTRepository()
        {
        }


        #region Generate PPT
        /// <summary>
        /// Create PPT File   
        /// </summary>
        /// <param name="fileName">Output file name</param>
        /// <param name="templateFilePath">template file path</param>
        /// <param name="templateData">template data</param>
        /// <param name="tableData">table data</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> CreatePPTFileAsync(string fileName, string templateFilePath, PowerPointTemplate templateData, DynamicDictionary<int, dynamic> tableData)
        {
            try
            {
                string path = System.IO.Path.GetTempPath();
                string outputFilePath = CreateFilePath(path, fileName);
                string tempFileName = fileName + StringConstant.TemporaryFile + StringConstant.PPTFileExtantion;
                tempFileName = System.IO.Path.Combine(path, tempFileName);
                templateData.ParseTemplate(templateFilePath, tempFileName);

                //Generate table data
                GenerateTable(tableData, tempFileName, outputFilePath);

                //Generate memory stram data for file download
                var memory = await GenerateMemoryStreamAsync(outputFilePath);

                //Delete generate file from server
                File.Delete(tempFileName);
                File.Delete(outputFilePath);

                Tuple<string, MemoryStream> fileData = new Tuple<string, MemoryStream>(fileName + StringConstant.PPTFileExtantion, memory);
                return fileData;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create ppt file
        /// </summary>
        /// <param name="fileName">output file name</param>
        /// <param name="templateFilePath">template file path</param>
        /// <param name="templateData">template data</param>
        /// <param name="slideNo">slide no.</param>
        /// <param name="jsonTableData">json table data</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> CreatePPTFileAsync(string fileName, string templateFilePath, PowerPointTemplate templateData, int slideNo, JSONRoot jsonTableData)
        {
            try
            {
                string path = System.IO.Path.GetTempPath();
                string outputFileName = CreateFilePath(path, fileName);
                string outputFilePath = fileName + StringConstant.TemporaryFile + StringConstant.PPTFileExtantion;
                outputFilePath = System.IO.Path.Combine(path, outputFilePath);
                if (jsonTableData != null)
                {
                    templateData.ParseTemplate(templateFilePath, outputFilePath);
                    //Generate json table data
                    for (int i = 0; i < jsonTableData.data.Count; i++)
                    {
                        jsonTableData.data[i].RowData[0] = (i + 1).ToString();
                    }
                    GenerateJsonTable(outputFilePath, outputFileName, jsonTableData, slideNo);

                }
                else
                {
                    templateData.ParseTemplate(templateFilePath, outputFileName);
                }
                //Generate memory stram data for file download
                var memory = await GenerateMemoryStreamAsync(outputFileName);

                //Delete generate file from server
                File.Delete(outputFilePath);
                File.Delete(outputFileName);

                Tuple<string, MemoryStream> fileData = new Tuple<string, MemoryStream>(fileName + StringConstant.PPTFileExtantion, memory);
                return fileData;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Common methods
        /// <summary>
        /// Create output file path 
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="fileName">File name</param>
        /// <returns>Path create file </returns>
        private string CreateFilePath(string path, string fileName)
        {
            string pptFileName = fileName + StringConstant.PPTFileExtantion;
            return System.IO.Path.Combine(path, pptFileName);
        }


        /// <summary>
        /// Generate memory stream data for download file
        /// </summary>
        /// <param name="fileName">file Name</param>
        /// <returns>return memory stream</returns>
        private async Task<MemoryStream> GenerateMemoryStreamAsync(string fileName)
        {
            var memoryStream = new MemoryStream();

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        /// <summary>
        /// Generate table data
        /// </summary>
        /// <typeparam name="T">Generic data type</typeparam>
        /// <param name="tableData">table data</param>
        /// <param name="templatePath">ppt template path</param>
        /// <param name="outputFilePath">Output file path</param>
        private void GenerateTable(DynamicDictionary<int, dynamic> tableData, string templatePath, string outputFilePath)
        {
            DynamicDictionary<int, dynamic>.KeyCollection keys = tableData.Keys;
            keyList = new List<int>();
            //create list of keys- convert Dynamic key in string format
            foreach (int key in keys)
            {
                keyList.Add(key);
            }

            var overflow = false;
            const int pageBorder = 3000000;
            var products = tableData;

            File.Copy(templatePath, outputFilePath, true);

            using (var myPres = PresentationDocument.Open(outputFilePath, true))
            {
                var presPart = myPres.PresentationPart;
                var slideIdList = presPart.Presentation.SlideIdList;

                var list = slideIdList.ChildElements
                            .Cast<SlideId>()
                            .Select(x => presPart.GetPartById(x.RelationshipId))
                            .Cast<SlidePart>();
                for (int i = 0; i < keyList.Count; i++)
                {
                    var tableSlidePart = (SlidePart)list.ToList()[keyList[i] - 1];
                    var current = tableSlidePart;
                    long totalHeight = 0;

                    List<dynamic> data = tableData.Where(x => x.Key == keyList[i]).SingleOrDefault().Value.ToList();
                    List<dynamic> tableList = new List<dynamic>(data.ToList()[0]);

                    foreach (var table in tableList)
                    {

                        if (overflow)
                        {
                            var newTablePart = CloneSlidePart(presPart, tableSlidePart);
                            current = newTablePart;
                            overflow = false;
                            totalHeight = 0;
                        }

                        var tbl = current.Slide.Descendants<Table>().FirstOrDefault();
                        var tr = new TableRow();
                        tr = GenerateRowData(table);
                        if (tbl != null)
                        {
                            tbl.Append(tr);
                        }

                        totalHeight += tr.Height;

                        if (totalHeight > pageBorder)
                            overflow = true;
                    }
                }

            }
        }

        /// <summary>
        /// If data exist from current slide height then create new clone side
        /// </summary>
        /// <param name="presentationPart">presentation part</param>
        /// <param name="slideTemplate">Slide part</param>
        /// <returns>Slide part</returns>
        static SlidePart CloneSlidePart(PresentationPart presentationPart, SlidePart slideTemplate)
        {
            //Create a new slide part in the presentation 
            SlidePart newSlidePart = presentationPart.AddNewPart<SlidePart>("newSlide" + index);
            index++;
            //Add the slide template content into the new slide 
            newSlidePart.FeedData(slideTemplate.GetStream(FileMode.Open));
            //make sure the new slide references the proper slide layout 
            newSlidePart.AddPart(slideTemplate.SlideLayoutPart);
            //Get the list of slide ids 
            SlideIdList slideIdList = presentationPart.Presentation.SlideIdList;
            //Figure out where to add the next slide (find max slide) 
            uint maxSlideId = 1;
            SlideId prevSlideId = null;
            foreach (SlideId slideId in slideIdList.ChildElements)
            {
                if (slideId.Id > maxSlideId)
                {
                    maxSlideId = slideId.Id;
                    prevSlideId = slideId;
                }
            }
            maxSlideId++;
            //Add new slide at the end of the deck 
            SlideId newSlideId = slideIdList.InsertAfter(new SlideId(), prevSlideId);
            //Make sure id and relid is set appropriately 
            newSlideId.Id = maxSlideId;
            newSlideId.RelationshipId = presentationPart.GetIdOfPart(newSlidePart);
            return newSlidePart;
        }

        /// <summary>
        /// generating table child rows
        /// </summary>
        /// <typeparam name="T">Generic Datatype</typeparam>
        /// <param name="tableData">table data </param>
        /// <returns>Create row for exported data</returns>
        private TableRow GenerateRowData<T>(T tableData)
        {
            Type type = tableData.GetType();
            PropertyInfo[] props = type.GetProperties();

            var tr = new TableRow();
            tr.Height = 200000;
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].GetValue(tableData) != null)
                {
                    tr.Append(CreateTextCell(props[i].GetValue(tableData).ToString()));
                }
            }
            return tr;
        }

        /// <summary>
        /// Create cell for table
        /// </summary>
        /// <param name="text">cell text data</param>
        /// <returns>table cell with data</returns>
        private static TableCell CreateTextCell(string text)
        {
            var textCol = new string[2];
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Length > 25)
                {
                    textCol[0] = text.Substring(0, 25);
                    textCol[1] = text.Substring(26);
                }
                else
                {
                    textCol[0] = text;
                }
            }
            else
            {
                textCol[0] = string.Empty;
            }


            TableCell tableCell3 = new TableCell();

            DocumentFormat.OpenXml.Drawing.TextBody textBody3 = new DocumentFormat.OpenXml.Drawing.TextBody();
            BodyProperties bodyProperties3 = new BodyProperties();
            ListStyle listStyle3 = new ListStyle();

            textBody3.Append(bodyProperties3);
            textBody3.Append(listStyle3);


            var nonNull = textCol.Where(t => !string.IsNullOrEmpty(t)).ToList();

            foreach (var textVal in nonNull)
            {
                Paragraph paragraph3 = new Paragraph();
                Run run2 = new Run();

                RunProperties runProperties2 = new RunProperties() { Language = "en-US", Dirty = false, FontSize = 1100 };
                SolidFill solidFill1 = new SolidFill();
                RgbColorModelHex rgbColorModelHex1 = new RgbColorModelHex() { Val = "00338d" };
                solidFill1.Append(rgbColorModelHex1);
                runProperties2.Append(solidFill1);

                DocumentFormat.OpenXml.Drawing.Text text2 = new DocumentFormat.OpenXml.Drawing.Text();
                text2.Text = textVal;
                run2.Append(runProperties2);
                run2.Append(text2);
                paragraph3.Append(run2);
                textBody3.Append(paragraph3);
            }

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            tableCell3.Append(textBody3);
            tableCell3.Append(tableCellProperties3);

            return tableCell3;
        }

        /// <summary>
        /// Generate table data
        /// </summary>
        /// <param name="templatePath">ppt template path</param>
        /// <param name="outputFilePath">Output file path</param>
        /// <param name="tableData">json table data</param>
        /// <param name="slideNo">slide no.</param>
        private void GenerateJsonTable(string templatePath, string outputFilePath, JSONRoot tableData, int slideNo)
        {
            var overflow = false;
            const int pageBorder = 3000000;
            File.Copy(templatePath, outputFilePath, true);

            using (var myPres = PresentationDocument.Open(outputFilePath, true))
            {
                var presPart = myPres.PresentationPart;
                var slideIdList = presPart.Presentation.SlideIdList;

                var list = slideIdList.ChildElements
                            .Cast<SlideId>()
                            .Select(x => presPart.GetPartById(x.RelationshipId))
                            .Cast<SlidePart>();

                var tableSlidePart = (SlidePart)list.ToList()[slideNo - 1];
                var current = tableSlidePart;
                long totalHeight = 0;

                if (overflow)
                {
                    var newTablePart = CloneSlidePart(presPart, tableSlidePart);
                    current = newTablePart;
                    overflow = false;
                    totalHeight = 0;
                }
                var tbl = current.Slide.Descendants<Table>().FirstOrDefault();
                if (tbl != null)
                {
                    var tr = new TableRow();
                    tr = GenerateRowData(tableData.columnNames);
                    tbl.Append(tr);

                    for (int i = 0; i < tableData.data.Count; i++)
                    {
                        tr = new TableRow();
                        tr = GenerateRowData(tableData.data[i].RowData);
                        tbl.Append(tr);
                    }
                    totalHeight += tr.Height;

                    if (totalHeight > pageBorder)
                        overflow = true;
                }
            }
        }

        /// <summary>
        /// generating table child rows
        /// </summary>
        /// <param name="tableData"> table data</param>
        /// <returns>Create row for exported data</returns>
        private TableRow GenerateRowData<T>(List<T> tableData)
        {

            var tr = new TableRow();
            tr.Height = 200000;
            for (int i = 0; i < tableData.Count; i++)
            {
                if (tableData[i] != null)
                {
                    tr.Append(CreateTextCell(tableData[i].ToString()));
                }
            }
            return tr;
        }

        #endregion

        #region Generate PPT with repeated slide
        /// <summary>
        /// Create ppt file
        /// </summary>
        /// <param name="fileName">Source template file name</param>
        /// <param name="templateFilePath">source template file path</param>
        /// <param name="templateData">source template data</param>
        /// <param name="tableData">table data for template</param>
        /// <param name="repeatedTemplatePath"> Repeated template path</param>
        /// <param name="repeatedTemplateData">Repeated template data</param>
        /// <param name="lastPPTemplatePath">last template file path </param>
        /// <param name="lastSlidePPTFile">last template file name</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> CreatePPTFileAsync(string fileName, string templateFilePath, PowerPointTemplate templateData, DynamicDictionary<int, dynamic> tableData, string repeatedTemplatePath, List<PowerPointTemplate> repeatedTemplateData, string lastPPTemplatePath, string lastSlidePPTFile)
        {
            try
            {
                string tempPath = System.IO.Path.GetTempPath() + StringConstant.TemporaryPPTFolder;

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                string outputFileName = CreateFilePath(tempPath, fileName);
                string tempFileName = fileName + StringConstant.TemporaryFile + StringConstant.PPTFileExtantion;
                tempFileName = System.IO.Path.Combine(tempPath, tempFileName);
                templateData.ParseTemplate(templateFilePath, tempFileName);

                //Generate table data
                string tableTempFile = tempPath + fileName + StringConstant.TemporaryTableFile + StringConstant.PPTFileExtantion;
                GenerateTable(tableData, tempFileName, tableTempFile);

                List<string> repeatedPPTNames = new List<string>();
                //create ppt with dynamic data slide
                for (int i = 0; i < repeatedTemplateData.Count; i++)
                {
                    string repeatedFileName = StringConstant.TemporaryObservationFile + i + StringConstant.PPTFileExtantion;
                    string tempFileNamePath = System.IO.Path.Combine(tempPath, repeatedFileName);
                    repeatedTemplateData[i].ParseTemplate(repeatedTemplatePath, tempFileNamePath);
                    repeatedPPTNames.Add(repeatedFileName);
                }

                // merge generated ppt with into one ppt
                string parentFileName = fileName + StringConstant.TemporaryTableFile + StringConstant.PPTFileExtantion;
                await MergeAllPPTAsync(tempPath, parentFileName, fileName, repeatedPPTNames, lastPPTemplatePath, lastSlidePPTFile);

                //Generate memory stram data for file download
                var memory = await GenerateMemoryStreamAsync(outputFileName);

                //Delete generate file from server
                DirectoryInfo directory = new DirectoryInfo(tempPath);
                //Determine whether the directory exists.
                if (Directory.Exists(tempPath))
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        file.Delete();
                    }
                    directory.Delete();
                }

                Tuple<string, MemoryStream> fileData = new Tuple<string, MemoryStream>(fileName + StringConstant.PPTFileExtantion, memory);
                return fileData;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Merge All ppt into single ppt file
        /// </summary>
        /// <param name="tempPath">temporary file path</param>
        /// <param name="parentFileName">Parent ppt file</param>
        /// <param name="outputFileName">output file name</param>
        /// <param name="sourcePresentations">List of presentation file name</param>
        /// <param name="lastPPTemplatePath">Last template file path</param>
        /// <param name="lastSlidePPTFile">Last template file name</param>
        ///<return>Task</return>
        public async Task MergeAllPPTAsync(string tempPath, string parentFileName, string outputFileName, List<string> sourcePresentations, string lastPPTemplatePath, string lastSlidePPTFile)
        {
            // Make a copy of the template presentation. This will throw an
            // exception if the template presentation does not exist.
            string tempOutput = outputFileName + "_final";
            File.Copy(tempPath + parentFileName, tempPath + tempOutput, true);

            // Loop through each source presentation and merge the slides 
            // into the merged presentation.
            foreach (string sourcePresentation in sourcePresentations)
                await MergeSlidesAsync(tempPath, sourcePresentation, tempOutput, tempPath);

            outputFileName = outputFileName + StringConstant.PPTFileExtantion;
            File.Copy(tempPath + tempOutput, tempPath + outputFileName, true);
            await MergeSlidesAsync(tempPath, lastSlidePPTFile, outputFileName, lastPPTemplatePath);
        }

        /// <summary>
        /// Merge ppt slides
        /// </summary>
        /// <param name="path">temporary folder path</param>
        /// <param name="sourcePresentation">sorce ppt file name</param>
        /// <param name="destPresentation">output file name</param>
        /// <param name="tempPath">temporary path </param>
        /// <returns>Task</returns>
        private async Task MergeSlidesAsync(string path, string sourcePresentation, string destPresentation, string tempPath)
        {
            int id = 0;

            // Open the destination presentation.
            using (PresentationDocument myDestDeck = PresentationDocument.Open(path + destPresentation, true))
            {
                PresentationPart destPresPart = myDestDeck.PresentationPart;

                // If the merged presentation does not have a SlideIdList 
                // element yet, add it.
                if (destPresPart.Presentation.SlideIdList == null)
                    destPresPart.Presentation.SlideIdList = new SlideIdList();

                // Open the source presentation. This will throw an exception if
                // the source presentation does not exist.
                using (PresentationDocument mySourceDeck = PresentationDocument.Open(tempPath + sourcePresentation, false))
                {
                    PresentationPart sourcePresPart = mySourceDeck.PresentationPart;

                    // Get unique ids for the slide master and slide lists
                    // for use later.
                    uniqueId = await GetMaxSlideMasterIdAsync(destPresPart.Presentation.SlideMasterIdList);

                    uint maxSlideId = await GetMaxSlideIdAsync(destPresPart.Presentation.SlideIdList);

                    // Copy each slide in the source presentation, in order, to 
                    // the destination presentation.
                    foreach (SlideId slideId in sourcePresPart.Presentation.SlideIdList)
                    {
                        SlidePart sp;
                        SlidePart destSp;
                        SlideMasterPart destMasterPart;
                        string relId;
                        SlideMasterId newSlideMasterId;
                        SlideId newSlideId;

                        // Create a unique relationship id.
                        id++;
                        sp = (SlidePart)sourcePresPart.GetPartById(slideId.RelationshipId);

                        relId = sourcePresentation.Remove(sourcePresentation.IndexOf('.')) + id;

                        // Add the slide part to the destination presentation.
                        destSp = destPresPart.AddPart<SlidePart>(sp, relId);

                        // The slide master part was added. Make sure the
                        // relationship between the main presentation part and
                        // the slide master part is in place.
                        destMasterPart = destSp.SlideLayoutPart.SlideMasterPart;
                        destPresPart.AddPart(destMasterPart);

                        // Add the slide master id to the slide master id list.
                        uniqueId++;
                        newSlideMasterId = new SlideMasterId();
                        newSlideMasterId.RelationshipId = destPresPart.GetIdOfPart(destMasterPart);
                        newSlideMasterId.Id = uniqueId;

                        destPresPart.Presentation.SlideMasterIdList.Append(newSlideMasterId);

                        // Add the slide id to the slide id list.
                        maxSlideId++;
                        newSlideId = new SlideId();
                        newSlideId.RelationshipId = relId;
                        newSlideId.Id = maxSlideId;

                        destPresPart.Presentation.SlideIdList.Append(newSlideId);
                    }

                    // Make sure that all slide layout ids are unique.
                    await FixSlideLayoutIdsAsync(destPresPart);
                }

                // Save the changes to the destination deck.
                destPresPart.Presentation.Save();
            }
        }

        /// <summary>
        /// Fix layout of ppt slide
        /// </summary>
        /// <param name="presentationPart">presentation part</param>
        /// <returns>Task/returns>
        private async Task FixSlideLayoutIdsAsync(PresentationPart presentationPart)
        {
            // Make sure that all slide layouts have unique ids.
            foreach (SlideMasterPart slideMasterPart in presentationPart.SlideMasterParts)
            {
                foreach (SlideLayoutId slideLayoutId in slideMasterPart.SlideMaster.SlideLayoutIdList)
                {
                    uniqueId++;
                    slideLayoutId.Id = (uint)uniqueId;
                }
                slideMasterPart.SlideMaster.Save();
            }
        }

        /// <summary>
        /// Get maximum slide layout id from ppt
        /// </summary>
        /// <param name="slideIdList">SlideIdList </param>
        /// <returns>Maximum slide layout id</returns>
        private async Task<uint> GetMaxSlideIdAsync(SlideIdList slideIdList)
        {
            // Slide identifiers have a minimum value of greater than or
            // equal to 256 and a maximum value of less than 2147483648. 
            uint max = 256;

            if (slideIdList != null)
                // Get the maximum id value from the current set of children.
                foreach (SlideId child in slideIdList.Elements<SlideId>())
                {
                    uint id = child.Id;

                    if (id > max)
                        max = id;
                }

            return max;
        }

        /// <summary>
        /// Get max slide master id 
        /// </summary>
        /// <param name="slideMasterIdList">master slide id</param>
        /// <returns>Maximum slide master id</returns>
        private async Task<uint> GetMaxSlideMasterIdAsync(SlideMasterIdList slideMasterIdList)
        {
            // Slide master identifiers have a minimum value of greater than
            // or equal to 2147483648. 
            uint max = 2147483648;

            if (slideMasterIdList != null)
                // Get the maximum id value from the current set of children.
                foreach (SlideMasterId child in
                  slideMasterIdList.Elements<SlideMasterId>())
                {
                    uint id = child.Id;

                    if (id > max)
                        max = id;
                }

            return max;
        }
        #endregion
    }
}
