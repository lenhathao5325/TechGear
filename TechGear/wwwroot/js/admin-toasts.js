/* Global admin toast helper (pure JS, uses Bootstrap Icons) */
(function(window, document){
    function createIcon(type){
        switch(type){
            case 'success': return '<i class="bi bi-check-circle-fill toast-icon" aria-hidden="true"></i>';
            case 'warning': return '<i class="bi bi-exclamation-triangle-fill toast-icon" aria-hidden="true"></i>';
            case 'info': return '<i class="bi bi-info-circle-fill toast-icon" aria-hidden="true"></i>';
            case 'error': default: return '<i class="bi bi-x-circle-fill toast-icon" aria-hidden="true"></i>';
        }
    }

    function showToast(type, message, title, timeout){
        timeout = typeof timeout === 'number' ? timeout : 4500;
        var container = document.getElementById('admin-toast-container');
        if(!container) return;

        var el = document.createElement('div');
        el.className = 'admin-toast admin-toast-'+(type||'info');
        el.setAttribute('role','status');
        el.innerHTML = `
            <div class="admin-toast-left">${createIcon(type)}</div>
            <div class="admin-toast-body">
                <div class="admin-toast-title">${title||''}</div>
                <div class="admin-toast-message">${message||''}</div>
                <div class="admin-toast-progress"><div class="admin-toast-progress-bar"></div></div>
            </div>
            <button type="button" class="admin-toast-close" aria-label="Close">&times;</button>`;

        container.appendChild(el);

        // start progress animation
        window.requestAnimationFrame(function(){
            var bar = el.querySelector('.admin-toast-progress-bar');
            if(bar){ bar.style.transition = 'width '+(timeout/1000)+'s linear'; bar.style.width = '0%'; }
        });

        // auto-dismiss
        var hideTimeout = setTimeout(function(){ removeToast(el); }, timeout);

        // set initial width to full then shrink
        var bar = el.querySelector('.admin-toast-progress-bar');
        if(bar){ bar.style.width = '100%'; }

        // close button
        el.querySelector('.admin-toast-close').addEventListener('click', function(){
            clearTimeout(hideTimeout);
            removeToast(el);
        });

        // remove helper
        function removeToast(node){
            if(!node) return;
            node.classList.add('admin-toast-hide');
            setTimeout(function(){ try{ node.remove(); }catch(e){} }, 320);
        }
        return el;
    }

    // expose globally
    window.showToast = showToast;
    window.adminToasts = { show: showToast };

})(window, document);
