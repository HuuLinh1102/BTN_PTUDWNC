import React, { useEffect } from 'react';
import PostFilterPane from '../../Components/Admin/PostFilterPane';

const Post = () => {
  useEffect(() => {
    document.title = 'Danh sách';
  }, []);

  return (
    <>
      <h1>Danh sách bài viết</h1>
      <PostFilterPane />
    </>
  );
}

export default Post;