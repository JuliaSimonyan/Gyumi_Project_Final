document.addEventListener('DOMContentLoaded', function() {
    // Sidebar navigation
    const sidebarItems = document.querySelectorAll('.sidebar-nav li');
    const contentSections = document.querySelectorAll('.content-section');
    const pageTitle = document.getElementById('page-title');

    sidebarItems.forEach(item => {
        item.addEventListener('click', function() {
            const section = this.getAttribute('data-section');
            
            // Update active sidebar item
            sidebarItems.forEach(item => item.classList.remove('active'));
            this.classList.add('active');
            
            // Show corresponding content section
            contentSections.forEach(section => section.classList.remove('active'));
            document.getElementById(section).classList.add('active');
            
            // Update page title
            pageTitle.textContent = this.querySelector('span').textContent;
        });
    });

    // Toggle sidebar
    const toggleSidebarBtn = document.querySelector('.toggle-sidebar');
    const sidebar = document.querySelector('.sidebar');
    
    toggleSidebarBtn.addEventListener('click', function() {
        sidebar.classList.toggle('collapsed');
    });

    // Content tabs
    const contentTabs = document.querySelectorAll('.content-tab');
    const tabContents = document.querySelectorAll('.content-tab-content');
    
    contentTabs.forEach(tab => {
        tab.addEventListener('click', function() {
            const tabId = this.getAttribute('data-tab');
            
            // Update active tab
            contentTabs.forEach(tab => tab.classList.remove('active'));
            this.classList.add('active');
            
            // Show corresponding tab content
            tabContents.forEach(content => content.classList.remove('active'));
            document.getElementById(`${tabId}-content`).classList.add('active');
        });
    });

    // Settings navigation
    const settingsNavItems = document.querySelectorAll('.settings-nav li');
    const settingsPanels = document.querySelectorAll('.settings-panel');
    
    settingsNavItems.forEach(item => {
        item.addEventListener('click', function() {
            const panel = this.getAttribute('data-settings');
            
            // Update active nav item
            settingsNavItems.forEach(item => item.classList.remove('active'));
            this.classList.add('active');
            
            // Show corresponding settings panel
            settingsPanels.forEach(panel => panel.classList.remove('active'));
            document.getElementById(`${panel}-settings`).classList.add('active');
        });
    });

    // Select all checkbox
    const selectAllCheckbox = document.getElementById('select-all');
    const itemCheckboxes = document.querySelectorAll('.select-item');
    
    if (selectAllCheckbox) {
        selectAllCheckbox.addEventListener('change', function() {
            itemCheckboxes.forEach(checkbox => {
                checkbox.checked = this.checked;
            });
        });
    }

    // Modal functionality
    const addTripBtn = document.querySelector('.add-new-btn');
    const addTripModal = document.getElementById('addTripModal');
    const closeModalBtns = document.querySelectorAll('.close-modal, .btn-cancel');
    
    if (addTripBtn && addTripModal) {
        addTripBtn.addEventListener('click', function() {
            addTripModal.style.display = 'flex';
        });
        
        closeModalBtns.forEach(btn => {
            btn.addEventListener('click', function() {
                addTripModal.style.display = 'none';
            });
        });
        
        window.addEventListener('click', function(event) {
            if (event.target === addTripModal) {
                addTripModal.style.display = 'none';
            }
        });
    }

    // Initialize charts
    initCharts();
});

function initCharts() {
    // Bookings Chart
    const bookingsCtx = document.getElementById('bookingsChart');
    if (bookingsCtx) {
        new Chart(bookingsCtx, {
            type: 'line',
            data: {
                labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
                datasets: [{
                    label: 'Bookings',
                    data: [12, 19, 15, 17, 22, 30, 25],
                    borderColor: '#1a1a1a',
                    backgroundColor: 'rgba(26, 26, 26, 0.1)',
                    tension: 0.4,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            display: true,
                            color: 'rgba(0, 0, 0, 0.05)'
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    }

    // Destinations Chart
    const destinationsCtx = document.getElementById('destinationsChart');
    if (destinationsCtx) {
        new Chart(destinationsCtx, {
            type: 'bar',
            data: {
                labels: ['Gyumri', 'Yerevan', 'Dilijan', 'Sevan', 'Jermuk'],
                datasets: [{
                    label: 'Bookings',
                    data: [65, 59, 40, 35, 28],
                    backgroundColor: [
                        'rgba(26, 26, 26, 0.8)',
                        'rgba(26, 26, 26, 0.7)',
                        'rgba(26, 26, 26, 0.6)',
                        'rgba(26, 26, 26, 0.5)',
                        'rgba(26, 26, 26, 0.4)'
                    ],
                    borderColor: [
                        'rgba(26, 26, 26, 1)',
                        'rgba(26, 26, 26, 1)',
                        'rgba(26, 26, 26, 1)',
                        'rgba(26, 26, 26, 1)',
                        'rgba(26, 26, 26, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            display: true,
                            color: 'rgba(0, 0, 0, 0.05)'
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    }
}