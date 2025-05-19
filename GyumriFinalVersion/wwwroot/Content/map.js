document.addEventListener('DOMContentLoaded', function () {
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

    // Define the Kumayri district with its boundaries
    var district = {
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
    };

    // Define points of interest with categories based on the new content
    var pois = [
        // Sights
        {
            name: "Sev Berd Fortress (Black Fortress)",
            lat: 40.7980,
            lng: 43.8400,
            panelId: "sev-berd-panel",
            category: "sights",
            time: "Exterior generally accessible anytime",
            address: "Located on a hill to the west of Gyumri city center"
        },
        {
            name: "Mother Armenia Monument",
            lat: 40.7970,
            lng: 43.8520,
            panelId: "mother-armenia-panel",
            category: "sights",
            time: "Park generally open, monument accessible anytime",
            address: "Victory Park (Haghtanaki Aygi), Gyumri"
        },
        {
            name: "The Iron Fountain",
            lat: 40.7960,
            lng: 43.8450,
            panelId: "iron-fountain-panel",
            category: "sights",
            time: "Publicly accessible outdoor structure, viewable anytime",
            address: "Near Charles Aznavour Square, Gyumri"
        },
        {
            name: "Independence Square",
            lat: 40.7910,
            lng: 43.8460,
            panelId: "independence-square-panel",
            category: "sights",
            time: "Public square, accessible anytime",
            address: "Independence Square, Gyumri"
        },
        {
            name: "Gyumri Bazar (Shuka)",
            lat: 40.7890,
            lng: 43.8470,
            panelId: "gyumri-bazar-panel",
            category: "sights",
            time: "8:00 AM - 5:00 PM",
            address: "Garegin Nzhdeh Ave, Gyumri"
        },
        {
            name: "Vardanants Square",
            lat: 40.7930,
            lng: 43.8440,
            panelId: "vardanants-square-panel",
            category: "sights",
            time: "Public square, accessible anytime",
            address: "Vardanants Square, Gyumri"
        },
        {
            name: "GTC (Gyumri Technology Center)",
            lat: 40.7940,
            lng: 43.8430,
            panelId: "gtc-panel",
            category: "sights",
            time: "9:00 AM - 6:00 PM, Mon-Fri",
            address: "Gayi Street 1, Gyumri"
        },
        {
            name: "TUMO Gyumri",
            lat: 40.7950,
            lng: 43.8470,
            panelId: "tumo-panel",
            category: "sights",
            time: "Specific program hours",
            address: "Central Park (Gorky Park), Gyumri"
        },

        // Museums
        {
            name: "Dzitoghtsyan Museum",
            lat: 40.7920,
            lng: 43.8430,
            panelId: "dzitoghtsyan-museum-panel",
            category: "museums",
            time: "11:00 AM - 5:00 PM (Closed Mondays)",
            address: "Victory Avenue 47, Gyumri"
        },
        {
            name: "Aslamazyan Sisters' Gallery",
            lat: 40.7910,
            lng: 43.8410,
            panelId: "aslamazyan-gallery-panel",
            category: "museums",
            time: "10:30 AM - 5:00 PM (Closed Mondays)",
            address: "Abovyan Street 240, Gyumri"
        },
        {
            name: "Mher Mkrtchyan Museum",
            lat: 40.7900,
            lng: 43.8420,
            panelId: "mkrtchyan-museum-panel",
            category: "museums",
            time: "11:00 AM - 5:00 PM (Closed Mondays)",
            address: "Rustaveli Street 30, Gyumri"
        },
        {
            name: "Avetik Isahakyan Museum",
            lat: 40.7895,
            lng: 43.8415,
            panelId: "isahakyan-museum-panel",
            category: "museums",
            time: "10:00 AM - 5:00 PM (Closed Mondays)",
            address: "Jivani Street 15, Gyumri"
        },
        {
            name: "Hovhannes Shiraz Museum",
            lat: 40.7885,
            lng: 43.8425,
            panelId: "shiraz-museum-panel",
            category: "museums",
            time: "10:00 AM - 5:00 PM (Closed Mondays)",
            address: "Varpetats Street 101, Gyumri"
        },

        // Churches
        {
            name: "Yot Verk Church",
            lat: 40.7925,
            lng: 43.8435,
            panelId: "yot-verk-panel",
            category: "churches",
            time: "Open for visitors",
            address: "Vardanants Square, Gyumri"
        },
        {
            name: "Holy Saviors Church",
            lat: 40.7935,
            lng: 43.8445,
            panelId: "holy-saviors-panel",
            category: "churches",
            time: "Open for visitors",
            address: "Vardanants Square, Gyumri"
        },
        {
            name: "Saint Nshan Church",
            lat: 40.7915,
            lng: 43.8425,
            panelId: "saint-nshan-panel",
            category: "churches",
            time: "Open for visitors",
            address: "Rustaveli Street, Gyumri"
        },

        // Cafes
        {
            name: "Aregak Bakery & Café",
            lat: 40.7910,
            lng: 43.8450,
            panelId: "aregak-bakery-panel",
            category: "cafes",
            time: "9:00 AM - 9:00 PM",
            address: "Abovyan Street 244/1, Gyumri"
        },
        {
            name: "Ponchik Monchik",
            lat: 40.7940,
            lng: 43.8460,
            panelId: "ponchik-monchik-panel",
            category: "cafes",
            time: "10:00 AM until late evenings",
            address: "Located in Bagratunyats Park & Main Square"
        },
        {
            name: "Cherkezi Dzor",
            lat: 40.7870,
            lng: 43.8380,
            panelId: "cherkezi-dzor-panel",
            category: "cafes",
            time: "9 AM to 11 PM",
            address: "Kars Highway, on the outskirts of Gyumri"
        },
        {
            name: "Sheeraz Cafe",
            lat: 40.7905,
            lng: 43.8440,
            panelId: "sheeraz-cafe-panel",
            category: "cafes",
            time: "10 AM - 12 AM",
            address: "31 Shiraz street"
        },
        {
            name: "Herbs & Honey Teashop",
            lat: 40.7920,
            lng: 43.8480,
            panelId: "herbs-honey-panel",
            category: "cafes",
            time: "9 AM - 11 PM",
            address: "5 Rijkov Street"
        },
        {
            name: "Gyumri Express Restaurant",
            lat: 40.7930,
            lng: 43.8470,
            panelId: "gyumri-express-panel",
            category: "cafes",
            time: "10 AM - 11 PM",
            address: "25 Gorki Street"
        },

        // Parks
        {
            name: "Central Park (Gorky Park)",
            lat: 40.7950,
            lng: 43.8460,
            panelId: "central-park-panel",
            category: "parks",
            time: "Public park, generally accessible 24/7",
            address: "Central Gyumri"
        },
        {
            name: "Friendship Park",
            lat: 40.7970,
            lng: 43.8480,
            panelId: "friendship-park-panel",
            category: "parks",
            time: "Public park, accessible anytime",
            address: "1/1, Garegin Nzhdeh Avenue"
        },
        {
            name: "Siremgy Mini-Park",
            lat: 40.7900,
            lng: 43.8420,
            panelId: "siremgy-park-panel",
            category: "parks",
            time: "Public park, accessible anytime",
            address: "Rustaveli Street, Gyumri"
        },

        // Entertainment
        {
            name: "Drama Theatre",
            lat: 40.7940,
            lng: 43.8450,
            panelId: "drama-theatre-panel",
            category: "entertainment",
            time: "Performance times vary",
            address: "4 Sayat-Nova Avenue"
        },
        {
            name: "Puppet Theatre",
            lat: 40.7920,
            lng: 43.8440,
            panelId: "puppet-theatre-panel",
            category: "entertainment",
            time: "Performance times vary",
            address: "Abovyan Street 232"
        },
        {
            name: "Varem & Marem Art Studio",
            lat: 40.7870,
            lng: 43.8430,
            panelId: "varem-marem-panel",
            category: "entertainment",
            time: "10 AM - 8 PM",
            address: "2 Ajemyan str"
        },
        {
            name: "Leo Art Studio",
            lat: 40.7860,
            lng: 43.8410,
            panelId: "leo-art-panel",
            category: "entertainment",
            time: "12 AM - 6 PM (Working days)",
            address: "5 Shiraz str"
        }
    ];

    // Add district polygon to the map
    var polygon = L.polygon(district.boundaries, {
        color: district.color,
        fillColor: district.fillColor,
        fillOpacity: district.fillOpacity,
        weight: 2
    }).addTo(map);

    // Add click event to show district info panel
    polygon.on('click', function () {
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
        }).addTo(map).on('click', function () {
            if (district.panelId) {
                showInfoPanel(district.panelId);
            } else {
                showDistrictLabel(district.name);
            }
        });
    }

    // Store all markers for filtering
    var allMarkers = {};
    var activeFilters = [];

    // Add POI markers to the map
    pois.forEach(function (poi) {
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
            marker.on('click', function () {
                showInfoPanel(poi.panelId);
            });
        } else {
            // For markers without panels, show a tooltip with basic info
            marker.on('click', function () {
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
        document.querySelectorAll('.info-panel').forEach(function (panel) {
            panel.style.display = 'none';
        });

        // Auto-hide the label after 3 seconds
        setTimeout(function () {
            label.style.display = 'none';
        }, 3000);
    }

    // Function to show info panel
    function showInfoPanel(panelId) {
        // Hide all panels first
        document.querySelectorAll('.info-panel').forEach(function (panel) {
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
    document.querySelectorAll('.close-btn').forEach(function (btn) {
        btn.addEventListener('click', function () {
            this.closest('.info-panel').style.display = 'none';
        });
    });

    // Filter markers based on active filters
    function filterMarkers() {
        // If no filters are active, show all markers
        if (activeFilters.length === 0) {
            Object.keys(allMarkers).forEach(function (category) {
                allMarkers[category].forEach(function (marker) {
                    marker.setOpacity(1);
                });
            });
            return;
        }

        // Hide all markers first
        Object.keys(allMarkers).forEach(function (category) {
            allMarkers[category].forEach(function (marker) {
                marker.setOpacity(0);
            });
        });

        // Show only markers in active categories
        activeFilters.forEach(function (category) {
            if (allMarkers[category]) {
                allMarkers[category].forEach(function (marker) {
                    marker.setOpacity(1);
                });
            }
        });
    }

    // Filter buttons functionality
    var filterButtons = document.querySelectorAll('.filter-btn');
    filterButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
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

    // Show Kumayri district panel by default when the page loads
    showInfoPanel('kumayri-district-panel');
});