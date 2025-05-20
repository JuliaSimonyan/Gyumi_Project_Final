document.addEventListener("DOMContentLoaded", () => {
    // Add this code at the beginning of the DOMContentLoaded event handler
    // to check if the initial location is Yerevan and show the bus section
    // Existing code...

    // Initial check for Yerevan to show/hide bus section
    const initialLocation = "Yerevan" // Default starting point

    // Get the bus group
    const busGroup = document.querySelector(".transport-option-group.bus-group")
    if (busGroup && initialLocation.toLowerCase().includes("yerevan")) {
        busGroup.style.display = "block"
    }

    // Get the train group
    const trainGroup = document.querySelector(".transport-option-group.train-group")
    if (trainGroup) {
        const trainLocations = [
            "yerevan",
            "noragavit",
            "masis",
            "sis",
            "haykashen",
            "ejmiatsin",
            "artashat",
            "armavir",
            "araqs",
            "dalarik",
            "qarakert",
            "arteni",
            "aragats",
            "ani",
            "bagravan",
            "jrapi",
            "shirakavan",
            "bayandur",
        ]

        if (trainLocations.some((location) => initialLocation.toLowerCase().includes(location))) {
            trainGroup.style.display = "block"
        }
    }

    // Rest of the existing code...
    console.log("DOM loaded, initializing map...")

    // Check if Leaflet is loaded
    if (typeof L === "undefined") {
        console.error("Leaflet library not loaded! Please check your script includes.")
        alert("Map library not loaded. Please refresh the page or check your internet connection.")
        return
    }

    // Get the map container
    const mapContainer = document.getElementById("map")
    if (!mapContainer) {
        console.error("Map container not found!")
        return
    }

    // Log map container dimensions for debugging
    console.log("Map container dimensions:", mapContainer.offsetWidth, mapContainer.offsetHeight)

    try {
        // Initialize the map centered on Gyumri, Armenia
        const gyumriCoords = [40.7942, 43.8453]
        const map = L.map("map", {
            zoomControl: true,
            attributionControl: false,
        }).setView(gyumriCoords, 8)

        console.log("Map initialized")

        // Add the base map layer (CartoDB Light)
        L.tileLayer("https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png", {
            attribution: "&copy; OpenStreetMap contributors & CartoDB",
            subdomains: "abcd",
            maxZoom: 19,
        }).addTo(map)

        console.log("Tile layer added")

        // Create a marker for Gyumri (destination)
        const gyumriMarker = L.marker(gyumriCoords, {
            title: "Gyumri",
            alt: "Gyumri",
        }).addTo(map)

        // Add a popup to the Gyumri marker
        gyumriMarker.bindPopup("<b>Gyumri</b><br>Your destination").openPopup()

        // Variables to track the current route
        let startMarker = null
        let routingControl = null
        let currentLocationName = "Yerevan"

        // Default starting point (Yerevan)
        const yerevanCoords = [40.1872, 44.5152]

        // Function to draw a route between two points
        function drawRoute(start, end, startLocationName) {
            console.log("Drawing route from", start, "to", end, "with name", startLocationName)

            // Remove existing routing control if it exists
            if (routingControl) {
                map.removeControl(routingControl)
            }

            // Create new routing control
            routingControl = L.Routing.control({
                waypoints: [L.latLng(start[0], start[1]), L.latLng(end[0], end[1])],
                routeWhileDragging: false,
                show: false, // Don't show the instructions panel
                lineOptions: {
                    styles: [{ color: "black", weight: 3, opacity: 0.7 }],
                },
                createMarker: () => null, // Don't create markers, we already have them
            }).addTo(map)

            // Update the current location name
            currentLocationName = startLocationName

            // Wait for the route to be calculated
            routingControl.on("routesfound", (e) => {
                const routes = e.routes
                const summary = routes[0].summary
                console.log("Route found:", summary)

                // Update the route info in the UI
                updateRouteInfo(startLocationName, summary.totalDistance)

                // Update the black info bar at the top
                updateInfoBar(startLocationName, summary.totalDistance)

                // Add this line inside the drawRoute function after updating the info bar:
                updateTransportOptions(startLocationName)
            })
        }

        // Function to update the route info in the UI
        function updateRouteInfo(startLocation, distance) {
            const routeTitle = document.getElementById("route-title")
            const routeDistance = document.getElementById("route-distance")

            if (routeTitle && routeDistance) {
                // Update the route title and distance
                routeTitle.textContent = `${startLocation} - Gyumri`
                routeDistance.textContent = `${Math.round(distance / 1000)} km`
            } else {
                console.error("Route info elements not found!")
            }
        }

        // Function to update the black info bar at the top
        function updateInfoBar(startLocation, distance) {
            const infoBarLocation = document.querySelector(".route-info-white h5")
            const infoBarDistance = document.querySelector(".route-info-white p")

            if (infoBarLocation && infoBarDistance) {
                // Update the location and distance
                infoBarLocation.innerHTML = `<i style="border: 1ps solid white; padding: 5px;" class="fa-solid fa-location-dot"></i> ${startLocation}`
                infoBarDistance.textContent = `You're ${Math.round(distance / 1000)} km away from Gyumri`
            } else {
                console.error("Info bar elements not found!")
            }
        }

        // Function to reverse geocode coordinates to get location name in English
        function reverseGeocode(lat, lng, callback) {
            console.log("Reverse geocoding", lat, lng)

            // Create a geocoder with English language preference
            const geocoder = L.Control.Geocoder.nominatim({
                geocodingQueryParams: {
                    "accept-language": "en", // Request English results
                    format: "json",
                },
            })

            geocoder.reverse({ lat: lat, lng: lng }, map.getZoom(), (results) => {
                let locationName = "Unknown Location"

                if (results && results.length > 0) {
                    // Try to get the English name from different properties
                    if (results[0].properties && results[0].properties.address) {
                        // Try to get a meaningful location name from the address components
                        const address = results[0].properties.address

                        // Prioritize city, town, village, etc.
                        if (address.city) {
                            locationName = address.city
                        } else if (address.town) {
                            locationName = address.town
                        } else if (address.village) {
                            locationName = address.village
                        } else if (address.county) {
                            locationName = address.county
                        } else if (address.state) {
                            locationName = address.state
                        } else if (results[0].name) {
                            locationName = results[0].name
                        }
                    } else if (results[0].name) {
                        locationName = results[0].name
                    }

                    // Ensure we have a clean English name
                    // This is a simple transliteration for common Armenian locations
                    const translationMap = {
                        Երևան: "Yerevan",
                        Գյումրի: "Gyumri",
                        Վանաձոր: "Vanadzor",
                        Էջմիածին: "Ejmiatsin",
                        Արտաշատ: "Artashat",
                        Արմավիր: "Armavir",
                        // Add more translations as needed
                    }

                    if (translationMap[locationName]) {
                        locationName = translationMap[locationName]
                    }

                    console.log("Location name found:", locationName)
                } else {
                    console.warn("No geocoding results found")
                }

                callback(locationName)
            })
        }

        // Initialize with a default route (Yerevan to Gyumri)
        console.log("Setting up default route")

        // Create a marker for Yerevan (starting point)
        startMarker = L.marker(yerevanCoords, {
            title: "Yerevan",
            alt: "Yerevan",
            draggable: true, // Allow the marker to be dragged
        }).addTo(map)

        // Add a popup to the Yerevan marker
        startMarker.bindPopup("<b>Yerevan</b><br>Your starting point").openPopup()

        // Draw the route from Yerevan to Gyumri
        drawRoute(yerevanCoords, gyumriCoords, "Yerevan")

        // Also add this line after the initial route is drawn (after the drawRoute call in the initialization):
        updateTransportOptions("Yerevan")

        // Fit the map to show both markers
        const bounds = L.latLngBounds([yerevanCoords, gyumriCoords])
        map.fitBounds(bounds, { padding: [50, 50] })

        // Add drag event to update route when marker is moved
        startMarker.on("dragend", (event) => {
            const marker = event.target
            const position = marker.getLatLng()
            console.log("Marker dragged to", position)

            // Reverse geocode to get location name
            reverseGeocode(position.lat, position.lng, (locationName) => {
                // Update route with new position
                drawRoute([position.lat, position.lng], gyumriCoords, locationName)
            })
        })

        // Add click event to the map
        map.on("click", (e) => {
            console.log("Map clicked at", e.latlng)

            // Remove existing start marker if it exists
            if (startMarker) {
                map.removeLayer(startMarker)
            }

            // Create a new marker at the clicked location
            startMarker = L.marker(e.latlng, {
                draggable: true,
            }).addTo(map)

            // Add drag event to update route when marker is moved
            startMarker.on("dragend", (event) => {
                const marker = event.target
                const position = marker.getLatLng()

                // Reverse geocode to get location name
                reverseGeocode(position.lat, position.lng, (locationName) => {
                    // Update route with new position
                    drawRoute([position.lat, position.lng], gyumriCoords, locationName)
                })
            })

            // Reverse geocode to get location name
            reverseGeocode(e.latlng.lat, e.latlng.lng, (locationName) => {
                // Add a popup to the marker
                startMarker.bindPopup(`<b>${locationName}</b><br>Your starting point`).openPopup()

                // Draw the route from the clicked location to Gyumri
                drawRoute([e.latlng.lat, e.latlng.lng], gyumriCoords, locationName)
            })
        })

        // Handle location search
        const searchInput = document.getElementById("location-search")
        const searchButton = document.getElementById("search-button")

        if (!searchInput || !searchButton) {
            console.error("Search elements not found!")
        } else {
            console.log("Search elements found and initialized")
        }

        // Function to search for a location
        function searchLocation() {
            const searchText = searchInput.value.trim()
            console.log("Searching for location:", searchText)

            if (searchText === "") {
                console.warn("Empty search text")
                return
            }

            // Use the geocoder to find the location with English results
            const geocoder = L.Control.Geocoder.nominatim({
                geocodingQueryParams: {
                    "accept-language": "en", // Request English results
                    format: "json",
                },
            })

            geocoder.geocode(searchText, (results) => {
                console.log("Geocoding results:", results)

                if (results && results.length > 0) {
                    // Get the first result
                    const result = results[0]
                    const latlng = result.center

                    // Use the search input value as the location name instead of the geocoded result
                    const locationName = searchText

                    console.log("Using location:", locationName, latlng)

                    // Remove existing start marker if it exists
                    if (startMarker) {
                        map.removeLayer(startMarker)
                    }

                    // Create a new marker at the found location
                    startMarker = L.marker(latlng, {
                        draggable: true,
                    }).addTo(map)

                    // Add a popup to the marker
                    startMarker.bindPopup(`<b>${locationName}</b><br>Your starting point`).openPopup()

                    // Draw the route from the found location to Gyumri
                    drawRoute([latlng.lat, latlng.lng], gyumriCoords, locationName)

                    // Fit the map to show both markers
                    const bounds = L.latLngBounds([latlng, gyumriCoords])
                    map.fitBounds(bounds, { padding: [50, 50] })

                    // Add drag event to update route when marker is moved
                    startMarker.on("dragend", (event) => {
                        const marker = event.target
                        const position = marker.getLatLng()

                        // When marker is dragged, use the original search text as the location name
                        // instead of doing reverse geocoding
                        drawRoute([position.lat, position.lng], gyumriCoords, locationName)
                    })
                } else {
                    console.error("Location not found")
                    alert("Location not found. Please try a different search term.")
                }
            })
        }

        // Add event listeners for search
        searchButton.addEventListener("click", () => {
            console.log("Search button clicked")
            searchLocation()
        })

        searchInput.addEventListener("keypress", (e) => {
            if (e.key === "Enter") {
                console.log("Enter key pressed in search")
                searchLocation()
            }
        })

        // Handle transport type selection
        document.querySelectorAll(".transport-header").forEach((header) => {
            header.addEventListener("click", function () {
                // Check if the parent group is disabled
                if (this.parentElement.classList.contains("disabled")) {
                    console.log("Clicked on disabled transport option")
                    return // Don't do anything if the group is disabled
                }

                // Toggle the expanded class
                this.classList.toggle("expanded")

                // Toggle the content visibility
                const content = this.nextElementSibling
                if (content.style.display === "none" || content.style.display === "") {
                    content.style.display = "block"
                    this.querySelector(".dropdown-arrow i").className = "fa-solid fa-chevron-up"
                } else {
                    content.style.display = "none"
                    this.querySelector(".dropdown-arrow i").className = "fa-solid fa-chevron-down"
                }
            })
        })

        // Make sure map is properly sized when window is resized
        window.addEventListener("resize", () => {
            console.log("Window resized, invalidating map size")
            if (map) {
                map.invalidateSize()
            }
        })

        // Force map to recalculate its size after a short delay
        setTimeout(() => {
            console.log("Forcing map resize")
            map.invalidateSize()
        }, 1000)

        // Add this function after the existing code in the DOMContentLoaded event listener
        // This will check if the bus and train options should be active based on location

        // Replace the updateTransportOptions function with this improved version
        function updateTransportOptions(locationName) {
            // Get the bus and train option groups
            const busGroup = document.querySelector(".transport-option-group.bus-group")
            const trainGroup = document.querySelector(".transport-option-group.train-group")

            if (busGroup) {
                // Bus is only active in Yerevan
                if (locationName.toLowerCase().includes("yerevan")) {
                    busGroup.classList.remove("disabled")
                    busGroup.querySelector(".transport-name").textContent = "Bus"
                    busGroup.style.display = "block"

                    // Make sure any unavailable message is removed
                    const unavailableMsg = busGroup.querySelector(".unavailable-message")
                    if (unavailableMsg) {
                        unavailableMsg.remove()
                    }
                } else {
                    busGroup.classList.add("disabled")
                    busGroup.querySelector(".transport-name").textContent = "Bus (not available)"

                    // Close the dropdown if it's open
                    const content = busGroup.querySelector(".transport-content")
                    if (content.style.display === "block") {
                        content.style.display = "none"
                        busGroup.querySelector(".transport-header").classList.remove("expanded")
                        busGroup.querySelector(".dropdown-arrow i").className = "fa-solid fa-chevron-down"
                    }

                    // Add unavailable message if it doesn't exist
                    if (!busGroup.querySelector(".unavailable-message")) {
                        const unavailableMsg = document.createElement("div")
                        unavailableMsg.className = "unavailable-message"
                        busGroup.appendChild(unavailableMsg)
                    }
                }
            }

            if (trainGroup) {
                // Train is only active in certain locations
                const trainLocations = [
                    "yerevan",
                    "noragavit",
                    "masis",
                    "sis",
                    "haykashen",
                    "ejmiatsin",
                    "artashat",
                    "armavir",
                    "araqs",
                    "dalarik",
                    "qarakert",
                    "arteni",
                    "aragats",
                    "ani",
                    "bagravan",
                    "jrapi",
                    "shirakavan",
                    "bayandur",
                ]

                const isTrainAvailable = trainLocations.some((location) => locationName.toLowerCase().includes(location))

                if (isTrainAvailable) {
                    trainGroup.classList.remove("disabled")
                    trainGroup.querySelector(".transport-name").textContent = "Train"
                    trainGroup.style.display = "block"

                    // Make sure any unavailable message is removed
                    const unavailableMsg = trainGroup.querySelector(".unavailable-message")
                    if (unavailableMsg) {
                        unavailableMsg.remove()
                    }
                } else {
                    trainGroup.classList.add("disabled")
                    trainGroup.querySelector(".transport-name").textContent = "Train (not available)"

                    // Close the dropdown if it's open
                    const content = trainGroup.querySelector(".transport-content")
                    if (content.style.display === "block") {
                        content.style.display = "none"
                        trainGroup.querySelector(".transport-header").classList.remove("expanded")
                        trainGroup.querySelector(".dropdown-arrow i").className = "fa-solid fa-chevron-down"
                    }

                    // Add unavailable message if it doesn't exist
                    if (!trainGroup.querySelector(".unavailable-message")) {
                        const unavailableMsg = document.createElement("div")
                        unavailableMsg.className = "unavailable-message"
                        trainGroup.appendChild(unavailableMsg)
                    }
                }
            }
        }

        // Call updateTransportOptions with the initial location
        updateTransportOptions("Yerevan")
    } catch (error) {
        console.error("Error initializing map:", error)
        alert("There was an error loading the map. Please refresh the page and try again.")
    }
})

