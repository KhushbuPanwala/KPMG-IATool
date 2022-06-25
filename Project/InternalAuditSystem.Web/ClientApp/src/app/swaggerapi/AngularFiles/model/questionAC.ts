/**
 * My Title
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.0.0
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
import { BaseModelAC } from './baseModelAC';
import { CheckboxQuestionAC } from './checkboxQuestionAC';
import { DropdownQuestionAC } from './dropdownQuestionAC';
import { FileUploadQuestionAC } from './fileUploadQuestionAC';
import { MultipleChoiceQuestionAC } from './multipleChoiceQuestionAC';
import { QuestionType } from './questionType';
import { RatingScaleQuestionAC } from './ratingScaleQuestionAC';
import { StrategicAnalysisAC } from './strategicAnalysisAC';
import { SubjectiveQuestionAC } from './subjectiveQuestionAC';
import { TextboxQuestionAC } from './textboxQuestionAC';
import { UserResponseAC } from './userResponseAC';
import { UserResponseDocumentAC } from './userResponseDocumentAC';

export interface QuestionAC extends BaseModelAC { 
    questionText?: string;
    type: QuestionType;
    isRequired: boolean;
    strategyAnalysisId: string;
    sortOrder: number;
    strategyAnalysis?: StrategicAnalysisAC;
    checkboxQuestion?: CheckboxQuestionAC;
    dropdownQuestion?: DropdownQuestionAC;
    fileUploadQuestion?: FileUploadQuestionAC;
    multipleChoiceQuestion?: MultipleChoiceQuestionAC;
    ratingScaleQuestion?: RatingScaleQuestionAC;
    subjectiveQuestion?: SubjectiveQuestionAC;
    textboxQuestion?: TextboxQuestionAC;
    options?: Array<string>;
    userResponse?: UserResponseAC;
    fileResponseList?: Array<UserResponseAC>;
    isUserResponseExists: boolean;
    userResponseDocumentACs?: Array<UserResponseDocumentAC>;
    questionType?: string;
    files?: Array<File>;
}
