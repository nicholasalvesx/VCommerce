document.addEventListener("DOMContentLoaded", () => {

    const mobileToggle = document.querySelector(".mobile-toggle")
    const sidebarToggle = document.querySelector("#sidebar-toggle")
    const sidebar = document.querySelector(".sidebar")
    const mainContent = document.querySelector(".main-content")

    if (mobileToggle && sidebar) {
        mobileToggle.addEventListener("click", () => {
            sidebar.classList.toggle("show")
            mainContent.classList.toggle("sidebar-open")
        })
    }

    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener("click", () => {
            sidebar.classList.toggle("show")
            mainContent.classList.toggle("sidebar-open")
        })
    }

    const dropdownToggle = document.querySelector(".dropdown-toggle")
    const dropdownMenu = document.querySelector(".dropdown-menu")

    if (dropdownToggle && dropdownMenu) {
        dropdownToggle.addEventListener("click", (e) => {
            e.preventDefault()
            dropdownMenu.classList.toggle("show")
        })

        document.addEventListener("click", (e) => {
            if (!dropdownToggle.contains(e.target) && !dropdownMenu.contains(e.target)) {
                dropdownMenu.classList.remove("show")
            }
        })
    }

    document.addEventListener("click", (e) => {
        if (window.innerWidth < 768) {
            if (!sidebar?.contains(e.target) && !mobileToggle?.contains(e.target) && !sidebarToggle?.contains(e.target)) {
                sidebar?.classList.remove("show")
                mainContent?.classList.remove("sidebar-open")
            }
        }
    })
})
