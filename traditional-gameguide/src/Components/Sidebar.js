import React from "react";
import SearchForm from "./SearchForm";
import CategoriesWidget from "./CategoriesWidget";
import FeaturedPost from "./FeaturePost";

const Sidebar = () => {
  return (
    <div className='pt-4 ps-2'>
      <SearchForm />

      <CategoriesWidget />

      <FeaturedPost />


    </div>
  )
}

export default Sidebar;