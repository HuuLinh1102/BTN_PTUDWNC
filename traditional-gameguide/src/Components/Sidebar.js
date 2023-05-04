import React from "react";
import SearchForm from "./SearchForm";

const Sidebar = () => {
  return (
    <div className='pt-4 ps-2'>
      <SearchForm />
      <h1>
        Chủ đề trò chơi
      </h1>
      <h1>
        Bài viết nổi bật
      </h1>
      <h1>
        Chia sẻ
      </h1>
    </div>
  )
}

export default Sidebar;