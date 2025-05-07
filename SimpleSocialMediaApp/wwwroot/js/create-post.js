// Function to check if either Content or MediaFiles has data
function checkForm() {
    var content = document.getElementById("Content").value; // Get the content value
    var mediaFiles = document.getElementById("MediaFiles").files.length; // Get the number of files selected

    // Enable or disable the submit button based on input values
    var submitButton = document.getElementById("submitButton");

    if (content.trim() !== "" || mediaFiles > 0) {
        submitButton.disabled = false; // Enable the button
    } else {
        submitButton.disabled = true; // Disable the button
    }
}

// Event listeners for the content textarea and media file input
document.getElementById("Content").addEventListener("input", checkForm);
document.getElementById("MediaFiles").addEventListener("change", checkForm);

// Run checkForm initially to disable the button if necessary
checkForm();