function validateForm() {
    var checkboxes = document.querySelectorAll('input[name="ParticipantsIds"]:checked');
    if (checkboxes.length === 0) {
        document.getElementById("error-message").style.display = "block";
        return false; // Prevent form submission
    }
    return true; // Allow form submission
}