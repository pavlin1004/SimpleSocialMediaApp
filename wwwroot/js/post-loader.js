let size = 5;  // Number of posts per request
let count = 1; // Track how many batches of posts have been loaded
let isLoading = false; // Prevent multiple simultaneous requests

// Function to load posts using the provided URL
function loadPosts(url) {
    if (isLoading) return; // Prevent duplicate calls
    isLoading = true;

    // Make a fetch request to load posts (or other content)
    fetch(`${url}?size=${size}&count=${count}`)
        .then(response => response.text())  // Expect HTML (partial view)
        .then(html => {
            // Append the new posts to the existing posts section
            document.querySelector('.posts-section').insertAdjacentHTML('beforeend', html);

            // Increment count for the next batch
            count++;

            // Allow new requests after posts are added
            isLoading = false;
        })
        .catch(error => {
            console.error('Error loading posts:', error);
            isLoading = false;
        });
}

// Function to check if the user has scrolled near the bottom of the page
function isNearBottom() {
    return window.innerHeight + window.scrollY >= document.body.offsetHeight - 100;
}

// Add scroll event listener to load more posts when near the bottom
window.addEventListener('scroll', () => {
    if (isNearBottom() && !isLoading) {
        // Get the URL from the data-url attribute of the load-more-trigger element
        const loadMoreUrl = document.getElementById('load-more-trigger').getAttribute('data-url');
        loadPosts(loadMoreUrl); // Call the function with the dynamic URL
    }
});

// Initial load of the first batch of posts when the page is loaded
window.addEventListener('DOMContentLoaded', () => {
    // Get the URL from the data-url attribute of the load-more-trigger element
    const loadMoreUrl = document.getElementById('load-more-trigger').getAttribute('data-url');
    loadPosts(loadMoreUrl); // Call the function with the dynamic URL
});