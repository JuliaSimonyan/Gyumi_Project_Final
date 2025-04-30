document.addEventListener('DOMContentLoaded', function() {
    // Initialize Leaflet
    var L = window.L;

    // Initialize the map
    var map = L.map('map', {
        zoomControl: false,
        attributionControl: false
    }).setView([40.7929, 43.8465], 14); // Gyumri coordinates

    // Add a grayscale tile layer
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        className: 'map-tiles-grayscale'
    }).addTo(map);

    // Add custom zoom control to bottom right
    L.control.zoom({
        position: 'bottomright'
    }).addTo(map);

    // Define the districts with their boundaries
    var districts = [
        {
            name: "Kumayri Historic District",
            id: "kumayri",
            panelId: "kumayri-district-panel",
            boundaries: [
                [40.7829, 43.8365],
                [40.7880, 43.8320],
                [40.7950, 43.8400],
                [40.8029, 43.8465],
                [40.8000, 43.8565],
                [40.7929, 43.8600],
                [40.7880, 43.8520],
                [40.7829, 43.8365]
            ],
            color: "#333333",
            fillColor: "#333333",
            fillOpacity: 0.4,
            icon: {
                lat: 40.7929,
                lng: 43.8465,
                html: '<div class="district-icon"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path><polyline points="9 22 9 12 15 12 15 22"></polyline></svg></div>'
            }
        },
        {
            name: "Northern District",
            id: "northern",
            panelId: "northern-district-panel",
            boundaries: [
                [40.8029, 43.8465],
                [40.8100, 43.8400],
                [40.8150, 43.8500],
                [40.8100, 43.8600],
                [40.8000, 43.8565]
            ],
            color: "#333333",
            fillColor: "#333333",
            fillOpacity: 0.4,
            icon: {
                lat: 40.8080,
                lng: 43.8500,
                html: '<div class="district-icon"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"></path></svg></div>'
            }
        },
        {
            name: "Eastern District",
            id: "eastern",
            panelId: "eastern-district-panel",
            boundaries: [
                [40.7929, 43.8600],
                [40.8000, 43.8565],
                [40.8100, 43.8600],
                [40.8050, 43.8700],
                [40.7950, 43.8650]
            ],
            color: "#333333",
            fillColor: "#333333",
            fillOpacity: 0.4,
            icon: {
                lat: 40.8000,
                lng: 43.8650,
                html: '<div class="district-icon"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M4 22V4c0-.5.2-1 .6-1.4C5 2.2 5.5 2 6 2h12c.5 0 1 .2 1.4.6.4.4.6.9.6 1.4v18H4z"></path><path d="M2 22h20"></path><path d="M10 6h4"></path><path d="M10 10h4"></path><path d="M10 14h4"></path><path d="M10 18h4"></path></svg></div>'
            }
        }
    ];

    // Define points of interest with categories
    var pois = [
        {
            name: "Leo Ceramics",
            lat: 40.7880,
            lng: 43.8420,
            panelId: "leo-ceramics-panel",
            category: "crafts",
            time: "12:00-18:00",
            address: "47 Haghtanaki Avenue, Gyumri"
        },
        {
            name: "Gyumri Museum",
            lat: 40.7920,
            lng: 43.8430,
            panelId: "gyumri-museum-panel",
            category: "museums",
            time: "10:00-17:00",
            address: "Eastern District, Gyumri"
        },
        {
            name: "Berlin Hotel",
            lat: 40.7950,
            lng: 43.8380,
            panelId: "berlin-hotel-panel",
            category: "hotels",
            time: "Open 24 hours",
            address: "Northern District, Gyumri"
        },
        {
            name: "Plaza Hotel",
            lat: 40.7900,
            lng: 43.8500,
            panelId: null,
            category: "hotels",
            time: "Open 24 hours",
            address: "Central Gyumri"
        },
        {
            name: "Gyumri Art Gallery",
            lat: 40.7940,
            lng: 43.8440,
            panelId: null,
            category: "museums",
            time: "10:00-18:00",
            address: "Vartanants Square, Gyumri"
        },
        {
            name: "Friendship Park",
            lat: 40.7970,
            lng: 43.8520,
            panelId: null,
            category: "outdoors",
            time: "9:00-22:00",
            address: "Abovyan St. 242, Gyumri"
        },
        {
            name: "Aregak Cafe",
            lat: 40.7910,
            lng: 43.8450,
            panelId: null,
            category: "cafes",
            time: "09:00-20:00",
            address: "Eastern District, Gyumri"
        },
        {
            name: "Cherkezi Dzor Restaurant",
            lat: 40.7890,
            lng: 43.8470,
            panelId: null,
            category: "restaurants",
            time: "11:00-23:00",
            address: "Shahumyan St., Gyumri"
        },
        {
            name: "Gyumri Ceramics Workshop",
            lat: 40.7860,
            lng: 43.8410,
            panelId: null,
            category: "crafts",
            time: "10:00-18:00",
            address: "Kumayri District, Gyumri"
        },
        {
            name: "Black Fortress",
            lat: 40.7980,
            lng: 43.8400,
            panelId: null,
            category: "history",
            time: "Open 24 hours",
            address: "Northern Gyumri"
        },
        {
            name: "Vartanants Square",
            lat: 40.7930,
            lng: 43.8440,
            panelId: null,
            category: "history",
            time: "Open 24 hours",
            address: "Central Gyumri"
        },
        {
            name: "Central Park",
            lat: 40.7950,
            lng: 43.8460,
            panelId: null,
            category: "outdoors",
            time: "7:00-23:00",
            address: "Central Gyumri"
        },
        {
            name: "Poloz Mukuch Restaurant",
            lat: 40.7870,
            lng: 43.8430,
            panelId: null,
            category: "restaurants",
            time: "11:00-23:00",
            address: "Kumayri District, Gyumri"
        },
        {
            name: "Herbs & Honey Cafe",
            lat: 40.7920,
            lng: 43.8480,
            panelId: null,
            category: "cafes",
            time: "09:00-21:00",
            address: "Eastern District, Gyumri"
        },
        {
            name: "Aslamazyan Sisters Gallery",
            lat: 40.7910,
            lng: 43.8410,
            panelId: null,
            category: "museums",
            time: "10:30-17:00",
            address: "25 Haghtanaki Avenue, Gyumri"
        }
    ];

    // Add district polygons to the map
    var districtPolygons = {};
    districts.forEach(function(district) {
        var polygon = L.polygon(district.boundaries, {
            color: district.color,
            fillColor: district.fillColor,
            fillOpacity: district.fillOpacity,
            weight: 2
        }).addTo(map);
        
        districtPolygons[district.id] = polygon;
        
        // Add click event to show district info panel
        polygon.on('click', function() {
            if (district.panelId) {
                showInfoPanel(district.panelId);
            } else {
                showDistrictLabel(district.name);
            }
        });
        
        // Add district icon if available
        if (district.icon) {
            L.marker([district.icon.lat, district.icon.lng], {
                icon: L.divIcon({
                    className: 'district-marker',
                    html: district.icon.html,
                    iconSize: [36, 36],
                    iconAnchor: [18, 18]
                })
            }).addTo(map).on('click', function() {
                if (district.panelId) {
                    showInfoPanel(district.panelId);
                } else {
                    showDistrictLabel(district.name);
                }
            });
        }
    });

    // Store all markers for filtering
    var allMarkers = {};
    var activeFilters = [];

    // Add POI markers to the map
    pois.forEach(function(poi) {
        // Create marker with the info pin icon
        var marker = L.marker([poi.lat, poi.lng], {
            icon: L.divIcon({
                className: 'custom-marker',
                html: '<div class="marker-icon"></div>',
                iconSize: [30, 30],
                iconAnchor: [15, 30]
            })
        }).addTo(map);
        
        // Add popup with name
        marker.bindTooltip(poi.name);
        
        // Add click event to show info panel if available
        if (poi.panelId) {
            marker.on('click', function() {
                showInfoPanel(poi.panelId);
            });
        } else {
            // For markers without panels, show a tooltip with basic info
            marker.on('click', function() {
                marker.bindPopup(
                    '<strong>' + poi.name + '</strong><br>' +
                    poi.time + '<br>' +
                    poi.address
                ).openPopup();
            });
        }
        
        // Store marker by category for filtering
        if (!allMarkers[poi.category]) {
            allMarkers[poi.category] = [];
        }
        allMarkers[poi.category].push(marker);
    });

    // Function to show district label
    function showDistrictLabel(districtName) {
        var label = document.getElementById('district-label');
        label.textContent = districtName.toUpperCase();
        label.style.display = 'block';
        
        // Hide all info panels
        document.querySelectorAll('.info-panel').forEach(function(panel) {
            panel.style.display = 'none';
        });
        
        // Auto-hide the label after 3 seconds
        setTimeout(function() {
            label.style.display = 'none';
        }, 3000);
    }

    // Function to show info panel
  // Function to show info panel
function showInfoPanel(panelId) {
    // Hide all panels first
    document.querySelectorAll('.info-panel').forEach(function(panel) {
        panel.style.display = 'none';
    });
    
    // Hide district label
    document.getElementById('district-label').style.display = 'none';
    
    // Show the selected panel
    var panel = document.getElementById(panelId);
    if (panel) {
        panel.style.display = 'block';
        
        // Make sure the map container adjusts to show both map and panel
        document.querySelector('.map-content').style.display = 'flex';
    }
}

    // Close button functionality for info panels
    document.querySelectorAll('.close-btn').forEach(function(btn) {
        btn.addEventListener('click', function() {
            this.closest('.info-panel').style.display = 'none';
        });
    });

    // Filter markers based on active filters
    function filterMarkers() {
        // If no filters are active, show all markers
        if (activeFilters.length === 0) {
            Object.keys(allMarkers).forEach(function(category) {
                allMarkers[category].forEach(function(marker) {
                    marker.setOpacity(1);
                });
            });
            return;
        }
        
        // Hide all markers first
        Object.keys(allMarkers).forEach(function(category) {
            allMarkers[category].forEach(function(marker) {
                marker.setOpacity(0);
            });
        });
        
        // Show only markers in active categories
        activeFilters.forEach(function(category) {
            if (allMarkers[category]) {
                allMarkers[category].forEach(function(marker) {
                    marker.setOpacity(1);
                });
            }
        });
    }

    // Filter buttons functionality
    var filterButtons = document.querySelectorAll('.filter-btn');
    filterButtons.forEach(function(btn) {
        btn.addEventListener('click', function() {
            // Toggle active class
            this.classList.toggle('active');
            
            // Get category from data attribute
            var category = this.getAttribute('data-category');
            
            // Toggle category in active filters
            var categoryIndex = activeFilters.indexOf(category);
            if (categoryIndex === -1) {
                activeFilters.push(category);
            } else {
                activeFilters.splice(categoryIndex, 1);
            }
            
            // Apply filters
            filterMarkers();
        });
    });
});