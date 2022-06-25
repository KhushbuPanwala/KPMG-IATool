export class Pagination<T> {
  totalRecords: number;
  pageIndex: number;
  pageSize: number;
  searchText: string;
  entityId: string;
  items: T[];
}

