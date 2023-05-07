import logo from './logo.svg';
import './App.css';
import Navbar from './Components/Navbar';
import Sidebar from './Components/Sidebar';
import Footer from './Components/Footer';
import Layout from './Pages/Layout';
import Index from './Pages/Index';
import Post from './Pages/Post';
import Accout from './Pages/Accout';
import Comment from './Pages/Comment';
import AdminLayout from './Pages/Admin/Layout';
import * as AdminIndex from './Pages/Admin/Index';
import {
  BrowserRouter as Router,
  Routes,
  Route,
} from 'react-router-dom';
import { Nav } from 'react-bootstrap';



function App() {
  return (
    <div>
      <Router>
        <Navbar />
        <div className='container-fluid'>
          <div className='row'>
            <div className='col-9'>
              <Routes>
                <Route path='/' element={<Layout />}>
                  <Route path='/' element={<Index />} />
                  <Route path='/blog' element={<Index />} />
                  <Route path='/blog/Post' element={<Post />} />
                  <Route path='blog/Accout' element={<Accout />} />
                  <Route path='blog/Comment' element={<Comment />} />
                </Route>
                <Route path='/admin' element={<AdminLayout />}>
                  <Route path='/admin' element={<AdminIndex.default />}/>
                  <Route path='/admin/Post' element={<Post />}/>
                  <Route path='/admin/Accout' element={<Accout/>}/>
                  <Route path='/admin/Comment' element={<Comment />}/>
                </Route>
              </Routes>
            </div>
            <div className='col-3 border-start'>
              <Sidebar />
            </div>
          </div>
        </div>
        <Footer />
      </Router>
    </div>
  );
}

export default App;
