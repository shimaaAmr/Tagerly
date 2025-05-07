
// Profile dropdown functionality
document.addEventListener('DOMContentLoaded', function() {
    const profileDropdown = document.querySelector('.profile-dropdown');
    
    profileDropdown.addEventListener('click', function(e) {
        this.classList.toggle('active');
    });
    
    // Close dropdown when clicking outside
    document.addEventListener('click', function(e) {
        if (!e.target.closest('.profile-dropdown')) {
            document.querySelector('.profile-dropdown').classList.remove('active');
        }
    });
});