/* SV22T1020073.Shop – Custom JavaScript
   ====================================== */

// ── Scroll to top button ────────────────────────────────
(function() {
    var btn = document.createElement('button');
    btn.id = 'scrollToTop';
    btn.innerHTML = '<i class="bi bi-chevron-up"></i>';
    btn.title = 'Cuộn lên đầu trang';
    btn.style.display = 'none';
    document.body.appendChild(btn);

    btn.addEventListener('click', function() {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    });

    window.addEventListener('scroll', function() {
        btn.style.display = window.scrollY > 200 ? 'flex' : 'none';
    });
})();

// ── Navbar scroll effect ────────────────────────────────
(function() {
    var navbar = document.querySelector('.shop-navbar');
    if (!navbar) return;
    window.addEventListener('scroll', function() {
        if (window.scrollY > 50) {
            navbar.classList.add('scrolled');
        } else {
            navbar.classList.remove('scrolled');
        }
    });
})();

// ── User Dropdown Manual Handler ─────────────────────────
(function() {
    document.addEventListener('click', function(e) {
        const btn = e.target.closest('#userDropdown');
        if (btn) {
            const menu = btn.nextElementSibling;
            if (menu && menu.classList.contains('dropdown-menu')) {
                // Let Bootstrap try first, but if it fails, this ensures reliability
                const isOpen = menu.classList.contains('show');
                if (!isOpen) {
                    // Force the menu to show if it didn't open
                    setTimeout(() => {
                        if (!menu.classList.contains('show')) {
                            const dropdownInstance = bootstrap.Dropdown.getOrCreateInstance(btn);
                            dropdownInstance.toggle();
                        }
                    }, 50);
                }
            }
        }
    });
})();

// ── Bootstrap validation enhancement ───────────────────
(function() {
    'use strict';
    var forms = document.querySelectorAll('.needs-validation');
    Array.prototype.slice.call(forms).forEach(function(form) {
        form.addEventListener('submit', function(event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
})();

// ── Utility: parse JSON safely ────────────────────────
function safeJson(res) {
    if (!res.ok) throw new Error('Server error: ' + res.status);
    return res.json();
}
