import { useState, useEffect, Fragment } from 'react';
import Form from 'react-bootstrap/Form';
import { Button } from 'react-bootstrap/Button';
import { Link } from 'react-router-dom';
import { getFilter } from '../../Services/BlogRepository';

const PostFilterPane = () => {
    const current = new Date(),
    [keyword, setKeyword] = useState('');
    const handleSubmit = (e) => {
        e.preventDefault();
    };

    return (
        <Form method='get'
            onSubmid={handleSubmit}
            className='row gy-2 gx-3 align-items-center p-2'>
            <Form.Group className='col-auto'>
                <Form.Label className='visually-hidden'>
                    keyword
                </Form.Label>
                <Form.Control
                    type='text'
                    placeholder='Nhập từ khóa...'
                    name='keyword'
                    value={keyword}
                    onChange={e => setKeyword(e.target.value)} />
                <Form.Group className='col-auto'>
                    <Button variant='primary' type='submit'>
                        Tìm/lọc
                    </Button>
                    <Link to='/admin/posts/edit' className='btn btn-success ms-2'>Thêm mới</Link>
                </Form.Group>
            </Form.Group>
        </Form>
    );
}

export default PostFilterPane;