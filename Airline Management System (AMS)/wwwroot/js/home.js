// ===================================
// HOME PAGE INTERACTIVITY
// ===================================

document.addEventListener('DOMContentLoaded', function() {
    
    // ===================================
    // DATE VALIDATION
    // ===================================
    const departureDateInput = document.getElementById('DepartureDate');
    const returnDateInput = document.getElementById('ReturnDate');

    // Set minimum date to today
    if (departureDateInput) {
        const today = new Date().toISOString().split('T')[0];
        departureDateInput.setAttribute('min', today);
        
        departureDateInput.addEventListener('change', function() {
            if (returnDateInput) {
                // Set return date minimum to departure date
                returnDateInput.setAttribute('min', this.value);
                
                // If return date is before departure date, clear it
                if (returnDateInput.value && returnDateInput.value < this.value) {
                    returnDateInput.value = '';
                }
            }
        });
    }

    if (returnDateInput) {
        const today = new Date().toISOString().split('T')[0];
        returnDateInput.setAttribute('min', today);
    }

    // ===================================
    // FORM VALIDATION
    // ===================================
    const searchForm = document.getElementById('flightSearchForm');
    
    if (searchForm) {
        searchForm.addEventListener('submit', function(e) {
            const origin = document.getElementById('Origin');
            const destination = document.getElementById('Destination');
            
            // Check if origin and destination are the same
            if (origin && destination && origin.value === destination.value) {
                e.preventDefault();
                alert('Origin and destination cannot be the same!');
                return false;
            }
            
            // Check if departure date is in the past
            if (departureDateInput) {
                const selectedDate = new Date(departureDateInput.value);
                const today = new Date();
                today.setHours(0, 0, 0, 0);
                
                if (selectedDate < today) {
                    e.preventDefault();
                    alert('Departure date cannot be in the past!');
                    return false;
                }
            }
            
            // Check if return date is before departure date
            if (returnDateInput && returnDateInput.value && departureDateInput) {
                const departDate = new Date(departureDateInput.value);
                const retDate = new Date(returnDateInput.value);
                
                if (retDate < departDate) {
                    e.preventDefault();
                    alert('Return date cannot be before departure date!');
                    return false;
                }
            }
        });
    }

    // ===================================
    // SMOOTH SCROLL
    // ===================================
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            const href = this.getAttribute('href');
            if (href !== '#' && href !== '') {
                e.preventDefault();
                const target = document.querySelector(href);
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    });

    // ===================================
    // SCROLL ANIMATIONS
    // ===================================
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.animation = 'fadeInUp 0.8s ease-out forwards';
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    // Observe elements
    document.querySelectorAll('.destination-card, .service-card, .testimonial-card').forEach(el => {
        el.style.opacity = '0';
        observer.observe(el);
    });

    // ===================================
    // DESTINATION CARDS INTERACTION
    // ===================================
    const destinationCards = document.querySelectorAll('.destination-card');
    
    destinationCards.forEach(card => {
        card.addEventListener('click', function() {
            const destination = this.dataset.destination;
            if (destination) {
                const destinationInput = document.getElementById('Destination');
                if (destinationInput) {
                    destinationInput.value = destination;
                    
                    // Scroll to search form
                    const searchContainer = document.querySelector('.flight-search-container');
                    if (searchContainer) {
                        searchContainer.scrollIntoView({
                            behavior: 'smooth',
                            block: 'center'
                        });
                        
                        // Focus on origin input
                        setTimeout(() => {
                            const originInput = document.getElementById('Origin');
                            if (originInput) {
                                originInput.focus();
                            }
                        }, 800);
                    }
                }
            }
        });
    });

    // ===================================
    // PASSENGER COUNT CONTROLS
    // ===================================
    const passengerInput = document.getElementById('PassengerCount');
    const decreaseBtn = document.getElementById('decreasePassengers');
    const increaseBtn = document.getElementById('increasePassengers');

    if (decreaseBtn && passengerInput) {
        decreaseBtn.addEventListener('click', function() {
            let currentValue = parseInt(passengerInput.value) || 1;
            if (currentValue > 1) {
                passengerInput.value = currentValue - 1;
            }
        });
    }

    if (increaseBtn && passengerInput) {
        increaseBtn.addEventListener('click', function() {
            let currentValue = parseInt(passengerInput.value) || 1;
            if (currentValue < 9) {
                passengerInput.value = currentValue + 1;
            }
        });
    }

    // ===================================
    // NAVBAR SCROLL EFFECT
    // ===================================
    const navbar = document.querySelector('.navbar');
    
    if (navbar) {
        window.addEventListener('scroll', function() {
            if (window.scrollY > 50) {
                navbar.classList.add('navbar-scrolled');
            } else {
                navbar.classList.remove('navbar-scrolled');
            }
        });
    }

    // ===================================
    // LOADING ANIMATION FOR SEARCH
    // ===================================
    const searchBtn = document.querySelector('.search-btn');
    
    if (searchBtn && searchForm) {
        searchForm.addEventListener('submit', function() {
            if (this.checkValidity()) {
                searchBtn.innerHTML = '<span class="spinner"></span> Searching...';
                searchBtn.disabled = true;
            }
        });
    }
});

// ===================================
// UTILITY FUNCTIONS
// ===================================

// Format date to readable string
function formatDate(dateString) {
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateString).toLocaleDateString('en-US', options);
}

// Validate email format
function isValidEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}
