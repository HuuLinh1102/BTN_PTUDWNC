import axios from "axios";

export async function getPosts() {
  try {
    const response = await
      axios.get('https://localhost:7054/api/posts');
    const data = response.data;
    if (data.isSuccess)
      return data.result;
    else
      return null;
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}