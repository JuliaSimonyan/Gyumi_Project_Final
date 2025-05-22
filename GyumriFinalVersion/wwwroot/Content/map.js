document.addEventListener("DOMContentLoaded", () => {
    // Initialize Leaflet
    var L = window.L

    // Initialize the map
    var map = L.map("map", {
        zoomControl: false,
        attributionControl: false,
    }).setView([40.7929, 43.8465], 15) // Gyumri coordinates with higher zoom

    // Add a grayscale tile layer
    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        maxZoom: 19,
        className: "map-tiles-grayscale",
    }).addTo(map)

    // Add custom zoom control to bottom right
    L.control
        .zoom({
            position: "bottomright",
        })
        .addTo(map)

    // Define the Kumayri district with its boundaries
    var district = {
        name: "Kumayri Historic District",
        id: "kumayri",
        panelId: "kumayri-district-panel",
        boundaries: [
            [40.7829, 43.8365],
            [40.788, 43.832],
            [40.795, 43.84],
            [40.8029, 43.8465],
            [40.8, 43.8565],
            [40.7929, 43.86],
            [40.788, 43.852],
            [40.7829, 43.8365],
        ],
        color: "#333333",
        fillColor: "#333333",
        fillOpacity: 0.4,
        icon: {
            lat: 40.7929,
            lng: 43.8465,
            html: '<div class="district-icon"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path><polyline points="9 22 9 12 15 12 15 22"></polyline></svg></div>',
        },
    }

    // Define points of interest with categories based on the new content
    var pois = [
        // Sights
        {
            name: "Sev Berd Fortress (Black Fortress)",
            lat: 40.7825201773008,
            lng: 43.825321947411496,
            panelId: "sev-berd-panel",
            category: "sights",
            time: "Exterior generally accessible anytime",
            address: "Located on a hill to the west of Gyumri city center",
        },
        {
            name: "Mother Armenia Monument",
            lat: 40.78271044740362,
            lng: 43.841033596055496,
            panelId: "mother-armenia-panel",
            category: "sights",
            time: "Park generally open, monument accessible anytime",
            address: "Victory Park (Haghtanaki Aygi), Gyumri",
        },
        {
            name: "The Iron Fountain",
            lat: 40.81577720719218,
            lng: 43.849757823043376,
            panelId: "iron-fountain-panel",
            category: "sights",
            time: "Publicly accessible outdoor structure, viewable anytime",
            address: "Near Charles Aznavour Square, Gyumri",
        },
        {
            name: "Independence Square",
            lat: 40.79318278937473,
            lng: 43.84715468256283,
            panelId: "independence-square-panel",
            category: "sights",
            time: "Public square, accessible anytime",
            address: "Independence Square, Gyumri",
        },
        {
            name: "Gyumri Bazar (Shuka)",
            lat: 40.799713421308276,
            lng: 43.847186872158524,
            panelId: "gyumri-bazar-panel",
            category: "sights",
            time: "8:00 AM - 5:00 PM",
            address: "Garegin Nzhdeh Ave, Gyumri",
        },
        {
            name: "Vardanants Square",
            lat: 40.785369811358166,
            lng: 43.84151983653486,
            panelId: "vardanants-square-panel",
            category: "sights",
            time: "Public square, accessible anytime",
            address: "Vardanants Square, Gyumri",
        },
        {
            name: "GTC (Gyumri Technology Center)",
            lat: 40.78674455413278,
            lng: 43.838236365370435,
            panelId: "gtc-panel",
            category: "sights",
            time: "9:00 AM - 6:00 PM, Mon-Fri",
            address: "Gayi Street 1, Gyumri",
        },
        {
            name: "TUMO Gyumri",
            lat: 40.78572261823523,
            lng: 43.836584996055585,
            panelId: "tumo-panel",
            category: "sights",
            time: "Specific program hours",
            address: "Central Park (Gorky Park), Gyumri",
        },

        // Museums
        {
            name: "Dzitoghtsyan Museum",
            lat: 40.7813242,
            lng: 43.8455645,
            panelId: "dzitoghtsyan-museum-panel",
            category: "museums",
            time: "11:00 AM - 5:00 PM (Closed Mondays)",
            address: "Victory Avenue 47, Gyumri",
        },
        {
            name: "Aslamazyan Sisters' Gallery",
            lat: 40.786536514701,
            lng: 43.840863513247605,
            panelId: "aslamazyan-gallery-panel",
            category: "museums",
            time: "10:30 AM - 5:00 PM (Closed Mondays)",
            address: "Abovyan Street 240, Gyumri",
            highlight: true, // Highlight this location with red color
        },
        {
            name: "Mher Mkrtchyan Museum",
            lat: 40.7887138,
            lng: 43.8428176,
            panelId: "mkrtchyan-museum-panel",
            category: "museums",
            time: "11:00 AM - 5:00 PM (Closed Mondays)",
            address: "Rustaveli Street 30, Gyumri",
        },
        {
            name: "Avetik Isahakyan Museum",
            lat: 40.7882933,
            lng: 43.8432826,
            panelId: "isahakyan-museum-panel",
            category: "museums",
            time: "10:00 AM - 5:00 PM (Closed Mondays)",
            address: "Jivani Street 15, Gyumri",
        },
        {
            name: "Hovhannes Shiraz Museum",
            lat: 40.78784808120083,
            lng: 43.842989736535,
            panelId: "shiraz-museum-panel",
            category: "museums",
            time: "10:00 AM - 5:00 PM (Closed Mondays)",
            address: "Varpetats Street 101, Gyumri",
        },

        // Churches
        {
            name: "Yot Verk Church",
            lat: 40.7862916087647,
            lng: 43.8421830788635,
            panelId: "yot-verk-panel",
            category: "churches",
            time: "Open for visitors",
            address: "Vardanants Square, Gyumri",
        },
        {
            name: "Holy Saviors Church",
            lat: 40.784274814439,
            lng: 43.84085551564372,
            panelId: "holy-saviors-panel",
            category: "churches",
            time: "Open for visitors",
            address: "Vardanants Square, Gyumri",
        },
        {
            name: "Saint Nshan Church",
            lat: 40.7888017941254,
            lng: 43.84161936722011,
            panelId: "saint-nshan-panel",
            category: "churches",
            time: "Open for visitors",
            address: "Rustaveli Street, Gyumri",
        },

        // Cafes
        {
            name: "Aregak Bakery & Café",
            lat: 40.7861588,
            lng: 43.8409795,
            panelId: "aregak-bakery-panel",
            category: "cafes",
            time: "9:00 AM - 9:00 PM",
            address: "Abovyan Street 244/1, Gyumri",
        },
        {
            name: "Ponchik Monchik",
            lat: 40.7877645,
            lng: 43.8453146,
            panelId: "ponchik-monchik-panel",
            category: "cafes",
            time: "10:00 AM until late evenings",
            address: "Located in Bagratunyats Park & Main Square",
        },
        {
            name: "Cherkezi Dzor",
            lat: 40.7975849,
            lng: 43.8278936,
            panelId: "cherkezi-dzor-panel",
            category: "cafes",
            time: "9 AM to 11 PM",
            address: "Kars Highway, on the outskirts of Gyumri",
        },
        {
            name: "Sheeraz Cafe",
            lat: 40.787618,
            lng: 43.8410295,
            panelId: "sheeraz-cafe-panel",
            category: "cafes",
            time: "10 AM - 12 AM",
            address: "31 Shiraz street",
        },
        {
            name: "Herbs & Honey Teashop",
            lat: 40.7863497,
            lng: 43.8437445,
            panelId: "herbs-honey-panel",
            category: "cafes",
            time: "9 AM - 11 PM",
            address: "5 Rijkov Street",
        },
        {
            name: "Gyumri Express Restaurant",
            lat: 40.787706,
            lng: 43.8401704,
            panelId: "gyumri-express-panel",
            category: "cafes",
            time: "10 AM - 11 PM",
            address: "25 Gorki Street",
        },

        // Parks
        {
            name: "Central Park (Gorky Park)",
            lat: 40.785208923778356,
            lng: 43.835489453726815,
            panelId: "central-park-panel",
            category: "parks",
            time: "Public park, generally accessible 24/7",
            address: "Central Gyumri",
        },
        {
            name: "Friendship Park",
            lat: 40.80551994447319,
            lng: 43.85082126722097,
            panelId: "friendship-park-panel",
            category: "parks",
            time: "Public park, accessible anytime",
            address: "1/1, Garegin Nzhdeh Avenue",
            highlight: true, // Highlight this location with red color
        },
        {
            name: "Siremgy Mini-Park",
            lat: 40.7892311,
            lng: 43.8385389,
            panelId: "siremgy-park-panel",
            category: "parks",
            time: "Public park, accessible anytime",
            address: "Rustaveli Street, Gyumri",
        },

        // Entertainment
        {
            name: "Drama Theatre",
            lat: 40.790863973083795,
            lng: 43.84518131324782,
            panelId: "drama-theatre-panel",
            category: "entertainment",
            time: "Performance times vary",
            address: "4 Sayat-Nova Avenue",
        },
        {
            name: "Puppet Theatre",
            lat: 40.78729981292692,
            lng: 43.841176980713165,
            panelId: "puppet-theatre-panel",
            category: "entertainment",
            time: "Performance times vary",
            address: "Abovyan Street 232",
        },
        {
            name: "Varem & Marem Art Studio",
            lat: 40.7853062,
            lng: 43.8370374,
            panelId: "varem-marem-panel",
            category: "entertainment",
            time: "10 AM - 8 PM",
            address: "2 Ajemyan str",
        },
        {
            name: "Leo Art Studio",
            lat: 40.78777398725779,
            lng: 43.83867490954881,
            panelId: "leo-art-panel",
            category: "entertainment",
            time: "12 AM - 6 PM (Working days)",
            address: "5 Shiraz str",
            highlight: true, // Highlight this location with red color
        },
    ]

    // Add district polygon to the map
    var polygon = L.polygon(district.boundaries, {
        color: district.color,
        fillColor: district.fillColor,
        fillOpacity: district.fillOpacity,
        weight: 2,
    }).addTo(map)

    // Add click event to show district info panel
    polygon.on("click", () => {
        if (district.panelId) {
            showInfoPanel(district.panelId)
        } else {
            showDistrictLabel(district.name)
        }
    })

    // Add district icon if available
    if (district.icon) {
        L.marker([district.icon.lat, district.icon.lng], {
            icon: L.divIcon({
                className: "district-marker",
                html: district.icon.html,
                iconSize: [36, 36],
                iconAnchor: [18, 18],
            }),
        })
            .addTo(map)
            .on("click", () => {
                if (district.panelId) {
                    showInfoPanel(district.panelId)
                } else {
                    showDistrictLabel(district.name)
                }
            })
    }

    // Store all markers for filtering
    var allMarkers = {}
    var activeFilters = []
    var poiMarkers = {} // Store markers by panel ID for easy access

    // Define regular and red marker icons
    var regularMarkerIcon = L.divIcon({
        className: "custom-marker",
        html: '<div class="marker-icon"></div>',
        iconSize: [30, 30],
        iconAnchor: [15, 30],
    })

    var redMarkerIcon = L.divIcon({
        className: "custom-marker",
        html: '<div class="red-marker-icon"></div>',
        iconSize: [30, 30],
        iconAnchor: [15, 30],
    })

    // Add POI markers to the map and make them visible by default
    pois.forEach((poi) => {
        // Choose the appropriate icon based on whether the location should be highlighted
        var markerIcon = poi.highlight ? redMarkerIcon : regularMarkerIcon

        // Create marker with the selected icon
        var marker = L.marker([poi.lat, poi.lng], {
            icon: markerIcon,
        }).addTo(map)

        // Add popup with name
        marker.bindTooltip(poi.name)

        // Add click event to show info panel if available
        if (poi.panelId) {
            marker.on("click", () => {
                showInfoPanel(poi.panelId)
            })

            // Store marker by panel ID for easy access
            poiMarkers[poi.panelId] = marker
        } else {
            // For markers without panels, show a tooltip with basic info
            marker.on("click", () => {
                marker.bindPopup("<strong>" + poi.name + "</strong><br>" + poi.time + "<br>" + poi.address).openPopup()
            })
        }

        // Store marker by category for filtering
        if (!allMarkers[poi.category]) {
            allMarkers[poi.category] = []
        }
        allMarkers[poi.category].push(marker)
    })

    // Function to show district label
    function showDistrictLabel(districtName) {
        var label = document.getElementById("district-label")
        label.textContent = districtName.toUpperCase()
        label.style.display = "block"

        // Hide all info panels
        document.querySelectorAll(".info-panel").forEach((panel) => {
            panel.style.display = "none"
        })

        // Auto-hide the label after 3 seconds
        setTimeout(() => {
            label.style.display = "none"
        }, 3000)
    }

    // Function to show info panel
    function showInfoPanel(panelId) {
        // Hide all panels first
        document.querySelectorAll(".info-panel").forEach((panel) => {
            panel.style.display = "none"
        })

        // Hide district label
        document.getElementById("district-label").style.display = "none"

        // Show the selected panel
        var panel = document.getElementById(panelId)
        if (panel) {
            panel.style.display = "block"

            // Make sure the map container adjusts to show both map and panel
            document.querySelector(".map-content").style.display = "flex"
        }
    }

    // Function to focus on a location without changing the info panel
    function focusOnLocation(panelId) {
        // Find the corresponding POI
        const poi = pois.find((p) => p.panelId === panelId)
        if (poi) {
            // Center the map on the location
            map.setView([poi.lat, poi.lng], 17) // Zoom in closer to the location

            // Make the marker bounce to highlight it
            if (poiMarkers[panelId]) {
                const marker = poiMarkers[panelId]

                // Add a temporary bounce animation class
                const icon = marker.getElement()
                if (icon) {
                    icon.classList.add("marker-bounce")

                    // Remove the animation class after it completes
                    setTimeout(() => {
                        icon.classList.remove("marker-bounce")
                    }, 1500) // Animation duration
                }

                // Open the tooltip
                marker.openTooltip()
            }
        }
    }

    // Close button functionality for info panels
    document.querySelectorAll(".close-btn").forEach((btn) => {
        btn.addEventListener("click", function () {
            this.closest(".info-panel").style.display = "none"
        })
    })

    // Filter markers based on active filters
    function filterMarkers() {
        // If no filters are active, show all markers
        if (activeFilters.length === 0) {
            Object.keys(allMarkers).forEach((category) => {
                allMarkers[category].forEach((marker) => {
                    marker.setOpacity(1)
                })
            })
            return
        }

        // Hide all markers first
        Object.keys(allMarkers).forEach((category) => {
            allMarkers[category].forEach((marker) => {
                marker.setOpacity(0)
            })
        })

        // Show only markers in active categories
        activeFilters.forEach((category) => {
            if (allMarkers[category]) {
                allMarkers[category].forEach((marker) => {
                    marker.setOpacity(1)
                })
            }
        })
    }

    // Filter buttons functionality
    var filterButtons = document.querySelectorAll(".filter-btn")
    filterButtons.forEach((btn) => {
        btn.addEventListener("click", function () {
            // Toggle active class
            this.classList.toggle("active")

            // Get category from data attribute
            var category = this.getAttribute("data-category")

            // Toggle category in active filters
            var categoryIndex = activeFilters.indexOf(category)
            if (categoryIndex === -1) {
                activeFilters.push(category)
            } else {
                activeFilters.splice(categoryIndex, 1)
            }

            // Apply filters
            filterMarkers()
        })
    })

    // Show all info points by default
    Object.keys(allMarkers).forEach((category) => {
        allMarkers[category].forEach((marker) => {
            marker.setOpacity(1)
        })
    })

    // Show Kumayri district panel by default when the page loads
    showInfoPanel("kumayri-district-panel")

    // Update the map center to better focus on the Kumayri district
    map.setView([40.791, 43.843], 15)

    // Make all info points visible by default
    Object.keys(allMarkers).forEach((category) => {
        allMarkers[category].forEach((marker) => {
            marker.setOpacity(1)
        })
    })

    // Add click functionality to featured location items
    document.querySelectorAll(".featured-location-link").forEach((item) => {
        item.addEventListener("click", function () {
            const panelId = this.getAttribute("data-panel-id")
            if (panelId) {
                // Focus on the location without changing the info panel
                focusOnLocation(panelId)
            }
        })

        // Add cursor pointer to indicate it's clickable
        item.style.cursor = "pointer"
    })
})
