
export interface JsonDocumentModel {
  tableId: string;
  columnNames: Array<string>;
  data: Array<RowDataObj>;
}

export interface RowDataObj {
  RowData: Array<string>;
}
