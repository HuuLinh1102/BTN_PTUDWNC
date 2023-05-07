import React, { useEffect } from 'react';

const Comment = () => {
  useEffect(() => {
    document.title = 'Bình luận';
  }, []);

  return (
    <h1>
      Đây là trang trang bình luận
    </h1>
  );
}

export default Comment;