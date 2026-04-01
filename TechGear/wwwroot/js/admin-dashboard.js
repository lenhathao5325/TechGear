// Admin Dashboard Enhancements
(function() {
    'use strict';

    // Initialize admin dashboard
    function initAdminDashboard() {
        // Add smooth scroll behavior
        document.documentElement.style.scrollBehavior = 'smooth';

        // Animate stat cards on scroll
        const cards = document.querySelectorAll('.stat-card');
        if (cards.length > 0) {
            observeElements(cards);
        }

        // Add hover effects to dashboard cards
        const dashboardCards = document.querySelectorAll('.dashboard-card');
        dashboardCards.forEach(card => {
            card.addEventListener('mouseenter', function() {
                this.style.borderColor = '#6366f1';
            });
            card.addEventListener('mouseleave', function() {
                this.style.borderColor = '#e2e8f0';
            });
        });

        // Enhance table rows
        const tableRows = document.querySelectorAll('.table-custom tbody tr');
        tableRows.forEach(row => {
            row.addEventListener('click', function() {
                // Could navigate to detail page
                console.log('Row clicked');
            });
        });
    }

    // Observe elements for animation
    function observeElements(elements) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.style.opacity = '1';
                    entry.target.style.animation = 'fadeInUp 0.5s ease forwards';
                }
            });
        }, {
            threshold: 0.1
        });

        elements.forEach(element => {
            element.style.opacity = '0';
            observer.observe(element);
        });
    }

    // Dashboard statistics animation
    function animateStatistics() {
        const stats = document.querySelectorAll('.stat-value');
        stats.forEach(stat => {
            const value = stat.textContent;
            if (!isNaN(value) && value.includes('.')) {
                // Animate currency values
                animateValue(stat, 0, parseFloat(value), 1000);
            }
        });
    }

    // Animate number values
    function animateValue(element, start, end, duration) {
        let current = start;
        const increment = (end - start) / (duration / 16);
        const timer = setInterval(() => {
            current += increment;
            if ((increment > 0 && current >= end) || (increment < 0 && current <= end)) {
                current = end;
                clearInterval(timer);
            }
            element.textContent = current.toFixed(0);
        }, 16);
    }

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initAdminDashboard);
    } else {
        initAdminDashboard();
    }

    // Add keyboard shortcuts
    document.addEventListener('keydown', function(e) {
        // Ctrl+Shift+S: Toggle sidebar
        if (e.ctrlKey && e.shiftKey && e.code === 'KeyS') {
            e.preventDefault();
            const btn = document.getElementById('sidebarToggle');
            if (btn) btn.click();
        }
    });

    // Responsive sidebar toggle
    function setupResponsiveSidebar() {
        const sidebarToggle = document.getElementById('sidebarToggle');
        if (sidebarToggle) {
            const hamburger = sidebarToggle.querySelector('.hamburger');

            sidebarToggle.addEventListener('click', function() {
                const isCollapsed = document.body.classList.toggle('sidebar-collapsed');
                // animate hamburger
                if (hamburger) hamburger.classList.toggle('is-active');
                // update aria
                const expanded = !isCollapsed;
                sidebarToggle.setAttribute('aria-expanded', expanded);
                localStorage.setItem('sidebarCollapsed', isCollapsed);
            });

            // Restore sidebar state
            if (localStorage.getItem('sidebarCollapsed') === 'true') {
                document.body.classList.add('sidebar-collapsed');
                if (sidebarToggle.querySelector('.hamburger')) sidebarToggle.querySelector('.hamburger').classList.remove('is-active');
                sidebarToggle.setAttribute('aria-expanded', 'false');
            } else {
                if (sidebarToggle.querySelector('.hamburger')) sidebarToggle.querySelector('.hamburger').classList.add('is-active');
                sidebarToggle.setAttribute('aria-expanded', 'true');
            }
        }
    }

    setupResponsiveSidebar();

})();
