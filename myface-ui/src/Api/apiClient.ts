export interface ListResponse<T> {
    items: T[];
    totalNumberOfItems: number;
    page: number;
    nextPage: string;
    previousPage: string;
}

export interface User {
    id: number;
    firstName: string;
    lastName: string;
    displayName: string;
    username: string;
    email: string;
    profileImageUrl: string;
    coverImageUrl: string;
}

export interface Interaction {
    id: number;
    user: User;
    type: string;
    date: string;
}

export interface Post {
    id: number;
    message: string;
    imageUrl: string;
    postedAt: string;
    postedBy: User;
    likes: Interaction[];
    dislikes: Interaction[];
}

export interface NewPost {
    message: string;
    imageUrl: string;
    // userId: number;
    // username: string;
    // password: string;
}

export interface HeaderInterface {
    crendential: string;
    // userId: string;
}

async function apiRequest(url: string, options: RequestInit = {}): Promise<Response> {
    const token = localStorage.getItem('authToken');
    const headers = {
        ...options.headers,
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json',
    };

    const response = await fetch(url, {
        ...options,
        headers
    });

    if (!response.ok) {
        throw new Error(`API request failed with status ${response.status}`);
    }

    return response;
}


export async function fetchUsers(searchTerm: string, page: number, pageSize: number): Promise<ListResponse<User>> {
    const response = await apiRequest(`https://localhost:5001/users?search=${searchTerm}&page=${page}&pageSize=${pageSize}`);
    return await response.json();
}

export async function fetchUser(userId: string | number): Promise<User> {
    const response = await apiRequest(`https://localhost:5001/users/${userId}`);
    return await response.json();
}

export async function fetchPosts(page: number, pageSize: number): Promise<ListResponse<Post>> {
    const response = await apiRequest(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}`);
    return await response.json();
}

export async function fetchPostsForUser(page: number, pageSize: number, userId: string | number) {
    const response = await apiRequest(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&postedBy=${userId}`);
    return await response.json();
}

export async function fetchPostsLikedBy(page: number, pageSize: number, userId: string | number) {
    const response = await apiRequest(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&likedBy=${userId}`);
    return await response.json();
}

export async function fetchPostsDislikedBy(page: number, pageSize: number, userId: string | number) {
    const response = await apiRequest(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&dislikedBy=${userId}`);
    return await response.json();
}


// export async function createPost(newPost: NewPost, header: HeaderInterface) {
export async function createPost(newPost: NewPost) {
    const response = await apiRequest(`https://localhost:5001/posts/create`, {
        method: "POST",
        body: JSON.stringify(newPost),
    });
    
    if (!response.ok) {
        throw new Error(await response.json())
    }
}

export async function login(username: string, password: string) {
    const response = await apiRequest(`https://localhost:5001/login`, {
        method: "POST",
        body: JSON.stringify({"Username" : username, "Password": password}),
    });
    
    if (!response.ok) {
        throw new Error(await response.json())
    }

    return await response.json();
}

export async function jwtlogin(username: string, password: string) {
    var token = localStorage.getItem('authToken');
    const response = await apiRequest(`https://localhost:5001/jwtlogin`, {
        method: "POST",
        body: JSON.stringify({"Username" : username, "Password": password}),
    });
    
    if (!response.ok) {
        throw new Error(await response.json())
    }

    return await response.json();
}