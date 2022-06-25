import { Injectable, EventEmitter, Output } from '@angular/core';
import { QuestionAC } from '../../swaggerapi/AngularFiles';


@Injectable({
  providedIn: 'root'
})
export class QuestionService {

  @Output() question: EventEmitter<QuestionAC> = new EventEmitter<QuestionAC>();
  @Output() questionToBeDeleted: EventEmitter<QuestionAC> = new EventEmitter<QuestionAC>();
  @Output() questionToBeUpdated: EventEmitter<QuestionAC> = new EventEmitter<QuestionAC>();
  @Output() questionToBePassed: EventEmitter<QuestionAC> = new EventEmitter<QuestionAC>();

  constructor() { }

  /**
   * To emit questionToBeAdded throughout
   * @param questionToBeAdded Question passed
   */
  onAddQuestion(questionToBeAdded: QuestionAC) {
    this.question.emit(questionToBeAdded);
  }

  /**
   * To emit questionToBeDeleted throughout
   * @param questionToBeAdded Question passed
   */
  onDeleteQuestion(questionToBeDeleted: QuestionAC) {
    this.questionToBeDeleted.emit(questionToBeDeleted);
  }

  /**
   * To emit questionToBeUpdated throughout
   * @param questionToBeAdded Question passed
   */
  onEditQuestion(questionToBeUpdated: QuestionAC) {
    this.questionToBeUpdated.emit(questionToBeUpdated);
  }

  /**
   * To emit questionToBePassed throughout
   * @param questionToBeAdded Question passed
   */
  onPassingQuestion(questionToBePassed: QuestionAC) {
    this.questionToBePassed.emit(questionToBePassed);
  }
}
