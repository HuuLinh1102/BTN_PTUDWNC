import { get_api } from "./Methods";

export function getPosts(keyword = '',
  pageSize = 10,
  pageNumber = 1,
  sortColumn = '',
  sortOrder = '') {
    return get_api(`https://localhost:7054/api/posts?keyword=${keyword}&PageSize=${pageSize}& PageNumber=${pageNumber}& SortColumn=${sortColumn}& SortOrder=${sortOrder} `);
  }

  export function getFilter() {
    return get_api('https://localhost:7054/api/posts/get-filter');
  }