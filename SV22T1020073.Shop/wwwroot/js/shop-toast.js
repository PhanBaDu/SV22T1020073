/**
 * Toast thông báo toàn shop (success / error / warning / info).
 */
(function (window) {
    'use strict';

    function escapeHtml(text) {
        if (text === null || text === undefined) return '';
        var div = document.createElement('div');
        div.textContent = String(text);
        return div.innerHTML;
    }

    var icons = {
        success: 'fa-check-circle',
        error: 'fa-circle-exclamation',
        warning: 'fa-triangle-exclamation',
        info: 'fa-circle-info'
    };

    /**
     * @param {string} message
     * @param {'success'|'error'|'warning'|'info'} [type]
     * @param {number} [durationMs] — 0 = không tự tắt
     */
    window.showShopToast = function (message, type, durationMs) {
        type = type || 'success';
        if (durationMs === undefined) {
            durationMs = type === 'error' ? 5500 : 4500;
        }

        document.querySelectorAll('.shop-toast').forEach(function (t) {
            t.classList.remove('show');
            setTimeout(function () { t.remove(); }, 280);
        });

        var el = document.createElement('div');
        el.className = 'shop-toast shop-toast--' + type;
        el.setAttribute('role', 'status');
        el.setAttribute('aria-live', 'polite');
        var ic = icons[type] || icons.info;
        el.innerHTML =
            '<i class="fas ' + ic + ' shop-toast__icon" aria-hidden="true"></i>' +
            '<span class="shop-toast__text">' + escapeHtml(String(message)) + '</span>' +
            '<button type="button" class="shop-toast__close" aria-label="Đóng">&times;</button>';

        document.body.appendChild(el);

        var hide = function () {
            el.classList.remove('show');
            setTimeout(function () { el.remove(); }, 320);
        };
        el.querySelector('.shop-toast__close').addEventListener('click', hide);

        requestAnimationFrame(function () {
            requestAnimationFrame(function () {
                el.classList.add('show');
            });
        });

        if (durationMs > 0) {
            setTimeout(hide, durationMs);
        }
        return el;
    };

    /** Đọc #shop-flash-json { success?, error?, warning?, info? } và bắn toast */
    window.showShopFlashFromDom = function () {
        var node = document.getElementById('shop-flash-json');
        if (!node || !node.textContent) return;
        try {
            var data = JSON.parse(node.textContent);
            var delay = 0;
            if (data.success) {
                setTimeout(function () { window.showShopToast(data.success, 'success'); }, delay);
                delay += 350;
            }
            if (data.error) {
                setTimeout(function () { window.showShopToast(data.error, 'error'); }, delay);
                delay += 350;
            }
            if (data.warning) {
                setTimeout(function () { window.showShopToast(data.warning, 'warning'); }, delay);
                delay += 350;
            }
            if (data.info) {
                setTimeout(function () { window.showShopToast(data.info, 'info'); }, delay);
            }
        } catch (e) { /* ignore */ }
        node.remove();
    };
})(window);
