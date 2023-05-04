import React, { useEffect } from 'react';

const Post = () => {
  useEffect(() => {
    document.title = 'Danh sách';
  }, []);

  return (
    <h1>
      Đây là trang danh sách trò chơi
    </h1>
  );
}

export default Post;