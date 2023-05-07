import { useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import { Link } from 'react-router-dom';
import { getPosts } from '../Services/FeaturedPosts';
const FeaturedPosts = () => {
  const [postList, setPostList] = useState([]);
  useEffect(() => {
    getPosts().then(data => {
      if (data)
        setPostList(data);
      else
        setPostList([]);
    });
  }, [])
  return (
    <div className='mb-4'>
      <h3 className='text-success mb-2'>
        Bài viết nổi bật
      </h3>
      {postList.length > 0 &&
        <ListGroup>
          {postList.map((item, index) => {
            return (
              <ListGroup.Item key={index}>
                <Link to={`/blog/category?slug=${item.urlSlug}`}
                  title={item.description}
                  key={index}>
                  {item.name}
                  <span>&nbsp;({item.postCount})</span>
                </Link>
              </ListGroup.Item>
            );
          })}
        </ListGroup>
      }
    </div>
  );
}
export default FeaturedPosts;