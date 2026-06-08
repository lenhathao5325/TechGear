/**
 * TechGear Chatbot - So sánh sản phẩm
 * Giao tiếp với API: /api/products/search và /api/products/compare
 */
(function () {
    'use strict';

    const API_BASE = 'http://localhost:5089';

    // ─── State ────────────────────────────────────────────────────────────────
    let compareQueue = [];      // { id, name, imageUrl }
    let searchResultCache = []; // kết quả search gần nhất

    // ─── DOM refs ──────────────────────────────────────────────────────────────
    const toggleBtn = document.getElementById('chatbot-toggle');
    const chatWindow = document.getElementById('chatbot-window');
    const closeBtn = document.getElementById('chatbot-close');
    const messages = document.getElementById('chatbot-messages');
    const input = document.getElementById('chatbot-input');
    const sendBtn = document.getElementById('chatbot-send');
    const pendingWrap = document.getElementById('chatbot-pending');

    if (!toggleBtn || !chatWindow) return;

    // ─── Toggle open/close ─────────────────────────────────────────────────────
    toggleBtn.addEventListener('click', () => {
        const isOpen = chatWindow.classList.toggle('open');
        toggleBtn.querySelector('i').className = isOpen ? 'fa fa-times' : 'fa fa-robot';
        if (isOpen && messages.children.length === 0) {
            greet();
        }
    });
    closeBtn.addEventListener('click', () => {
        chatWindow.classList.remove('open');
        toggleBtn.querySelector('i').className = 'fa fa-robot';
    });

    // ─── Send message ──────────────────────────────────────────────────────────
    sendBtn.addEventListener('click', handleSend);
    input.addEventListener('keydown', e => { if (e.key === 'Enter') handleSend(); });

    // ─── Chip suggestions ──────────────────────────────────────────────────────
    document.querySelectorAll('.chatbot-chip').forEach(chip => {
        chip.addEventListener('click', () => {
            input.value = chip.dataset.msg;
            handleSend();
        });
    });

    // ─── Greeting ──────────────────────────────────────────────────────────────
    function greet() {
        addBotMessage(
            'Xin chào! Tôi là <strong>TechBot</strong> 🤖<br>' +
            'Tôi có thể giúp bạn <b>tìm kiếm</b> và <b>so sánh sản phẩm</b>.<br>' +
            'Hãy thử: <em>"so sánh laptop gaming"</em> hoặc <em>"tìm chuột"</em>.'
        );
    }

    // ─── Main handler ──────────────────────────────────────────────────────────
    async function handleSend() {
        const text = input.value.trim();
        if (!text) return;
        input.value = '';
        addUserMessage(text);

        const lower = text.toLowerCase();

        // Lệnh xoá danh sách so sánh
        if (/^(xo[aá]|clear|reset|làm lại)/i.test(lower)) {
            compareQueue = [];
            renderPending();
            addBotMessage('Đã xoá danh sách so sánh. Bạn có thể bắt đầu lại!');
            return;
        }

        // Lệnh so sánh trực tiếp: "so sánh X với Y"
        const compareMatch = lower.match(/so\s*s[aá]nh\s+(.+?)(?:\s+v[oớ]i\s+(.+))?$/i);
        if (compareMatch) {
            const termA = compareMatch[1] ? compareMatch[1].trim() : null;
            const termB = compareMatch[2] ? compareMatch[2].trim() : null;
            showTyping();
            await handleCompareCommand(termA, termB);
            return;
        }

        // Lệnh tìm kiếm
        const searchMatch = lower.match(/(?:t[iì]m|tìm kiếm|search|xem)\s+(.+)/i);
        if (searchMatch) {
            showTyping();
            await doSearch(searchMatch[1].trim(), 'search');
            return;
        }

        // Thêm sản phẩm vào so sánh theo từ khoá
        if (/thêm|add/i.test(lower)) {
            const kw = lower.replace(/thêm|add/gi, '').trim();
            if (kw) {
                showTyping();
                await doSearch(kw, 'add');
                return;
            }
        }

        // So sánh queue hiện tại
        if (/^so\s*s[aá]nh$/.test(lower) || lower === 'compare') {
            if (compareQueue.length >= 2) {
                showTyping();
                await doCompare();
            } else {
                addBotMessage('Bạn cần thêm ít nhất <b>2 sản phẩm</b> vào danh sách so sánh trước.');
            }
            return;
        }

        // Fallback: tìm kiếm tổng quát
        showTyping();
        await doSearch(text, 'search');
    }

    // ─── Compare command handler ────────────────────────────────────────────────
    async function handleCompareCommand(termA, termB) {
        if (!termA) {
            removeTyping();
            addBotMessage('Vui lòng nhập tên sản phẩm muốn so sánh. Ví dụ: <em>so sánh laptop với màn hình</em>');
            return;
        }

        // Nếu chỉ có 1 từ khoá, tìm và thêm vào queue
        if (!termB) {
            await doSearch(termA, 'add');
            if (compareQueue.length >= 2) {
                await doCompare();
            } else {
                addBotMessage(`Đã thêm vào danh sách. Tìm thêm sản phẩm để so sánh, hoặc gõ <em>"so sánh"</em> khi đủ 2 sản phẩm.`);
            }
            return;
        }

        // Tìm đồng thời cả hai
        const [resA, resB] = await Promise.all([
            fetchSearch(termA),
            fetchSearch(termB)
        ]);
        removeTyping();

        if (!resA.length && !resB.length) {
            addBotMessage(`Không tìm thấy sản phẩm nào với từ khoá "<b>${termA}</b>" và "<b>${termB}</b>".`);
            return;
        }

        // Lấy sản phẩm đầu tiên của mỗi nhóm
        const pickA = resA[0] || null;
        const pickB = resB[0] || null;

        if (!pickA) {
            addBotMessage(`Không tìm thấy sản phẩm nào với "<b>${termA}</b>". Thử từ khoá khác nhé!`);
            return;
        }
        if (!pickB) {
            addBotMessage(`Không tìm thấy sản phẩm nào với "<b>${termB}</b>". Thử từ khoá khác nhé!`);
            return;
        }

        // Nếu có kết quả đơn nhất thì so sánh thẳng
        compareQueue = [
            { id: pickA.id, name: pickA.name, imageUrl: pickA.imageUrl },
            { id: pickB.id, name: pickB.name, imageUrl: pickB.imageUrl }
        ];

        if (resA.length > 1 || resB.length > 1) {
            // Còn nhiều kết quả, hỏi người dùng chọn
            addBotMessage(`Tìm thấy nhiều sản phẩm. Tôi sẽ so sánh <b>${pickA.name}</b> với <b>${pickB.name}</b>. Bạn có thể thay đổi bằng cách thêm lại.`);
        }

        showTyping();
        await doCompare();
    }

    // ─── Search ────────────────────────────────────────────────────────────────
    async function doSearch(keyword, mode) {
        const results = await fetchSearch(keyword);
        removeTyping();

        if (!results.length) {
            addBotMessage(`Không tìm thấy sản phẩm nào với "<b>${keyword}</b>". Thử từ khoá khác nhé!`);
            return;
        }

        searchResultCache = results;

        if (mode === 'add') {
            if (results.length === 1) {
                addToQueue(results[0]);
            } else {
                addBotMessage(buildSearchResultsHTML(results, true));
            }
        } else {
            addBotMessage(buildSearchResultsHTML(results, false));
        }
    }

    function buildSearchResultsHTML(results, showAddBtn) {
        let html = `<div>Tìm thấy <b>${results.length}</b> sản phẩm:</div>`;
        html += '<div style="margin-top:6px">';
        results.slice(0, 6).forEach(p => {
            const img = p.imageUrl ? `<img src="${escHtml(p.imageUrl)}" alt="">` : `<div style="width:38px;height:38px;background:#f5f5f5;border-radius:6px;display:flex;align-items:center;justify-content:center"><i class="fa fa-image text-muted"></i></div>`;
            const price = formatPrice(p.minPrice, p.maxPrice);
            html += `
            <div class="search-result-item" data-id="${p.id}">
                ${img}
                <div class="info flex-grow-1">
                    <div class="name">${escHtml(p.name)}</div>
                    <div class="meta">${escHtml(p.brandName || '')} · ${escHtml(p.categoryName || '')} · <span class="price-tag">${price}</span></div>
                </div>
                ${showAddBtn
                    ? `<button class="chatbot-chip" style="flex-shrink:0" onclick="window.techChatbot.addById(${p.id})"><i class="fa fa-plus"></i> Thêm</button>`
                    : `<a href="/Products/Details/${p.id}" target="_blank" class="chatbot-chip" style="flex-shrink:0">Xem</a>`
                }
            </div>`;
        });
        html += '</div>';
        if (showAddBtn) {
            html += `<div style="font-size:.75rem;color:#888;margin-top:4px">Nhấn <b>Thêm</b> để đưa vào danh sách so sánh</div>`;
        }
        return html;
    }

    // ─── Compare ───────────────────────────────────────────────────────────────
    async function doCompare() {
        if (compareQueue.length < 2) {
            removeTyping();
            addBotMessage('Cần ít nhất <b>2 sản phẩm</b> trong danh sách so sánh.');
            return;
        }

        const ids = compareQueue.map(p => p.id).join(',');
        try {
            const res = await fetch(`${API_BASE}/api/products/compare?ids=${ids}`);
            if (!res.ok) throw new Error('API error');
            const data = await res.json();
            removeTyping();
            addBotMessage(buildCompareTableHTML(data));
            // Gợi ý xem chi tiết
            const links = data.map(p => `<a href="/Products/Details/${p.id}" target="_blank" class="chatbot-chip">${escHtml(p.name)}</a>`).join(' ');
            addBotMessage(`Xem chi tiết: ${links}`);
        } catch {
            removeTyping();
            addBotMessage('Không thể tải dữ liệu so sánh. Vui lòng thử lại sau.');
        }
    }

    function buildCompareTableHTML(products) {
        if (!products || products.length < 2) return 'Không đủ dữ liệu để so sánh.';

        let html = `<div class="compare-table-wrap"><table class="compare-table"><thead><tr>
            <th class="row-label">Tiêu chí</th>`;
        products.forEach(p => {
            html += `<th>${escHtml(p.name)}</th>`;
        });
        html += '</tr></thead><tbody>';

        // Ảnh
        html += '<tr><td class="row-label">Ảnh</td>';
        products.forEach(p => {
            html += `<td>${p.imageUrl ? `<img class="product-img" src="${escHtml(p.imageUrl)}" alt="">` : '—'}</td>`;
        });
        html += '</tr>';

        // Thương hiệu
        html += '<tr><td class="row-label">Thương hiệu</td>';
        products.forEach(p => { html += `<td>${escHtml(p.brandName || '—')}</td>`; });
        html += '</tr>';

        // Danh mục
        html += '<tr><td class="row-label">Danh mục</td>';
        products.forEach(p => { html += `<td>${escHtml(p.categoryName || '—')}</td>`; });
        html += '</tr>';

        // Giá
        html += '<tr><td class="row-label">Giá</td>';
        products.forEach(p => {
            html += `<td><span class="price-tag">${formatPrice(p.minPrice, p.maxPrice)}</span></td>`;
        });
        html += '</tr>';

        // Tồn kho
        html += '<tr><td class="row-label">Tồn kho</td>';
        products.forEach(p => {
            const s = p.totalStock > 0
                ? `<span class="in-stock"><i class="fa fa-check"></i> Còn hàng</span>`
                : `<span class="out-stock"><i class="fa fa-times"></i> Hết hàng</span>`;
            html += `<td>${s}</td>`;
        });
        html += '</tr>';

        // Options (nếu có)
        const allOptions = {};
        products.forEach(p => {
            (p.options || []).forEach(o => {
                if (!allOptions[o.optionName]) allOptions[o.optionName] = {};
                allOptions[o.optionName][p.id] = (o.values || []).join(', ');
            });
        });
        Object.entries(allOptions).forEach(([optName, vals]) => {
            html += `<tr><td class="row-label">${escHtml(optName)}</td>`;
            products.forEach(p => {
                html += `<td>${escHtml(vals[p.id] || '—')}</td>`;
            });
            html += '</tr>';
        });

        html += '</tbody></table></div>';
        return html;
    }

    // ─── Queue helpers ─────────────────────────────────────────────────────────
    function addToQueue(product) {
        if (compareQueue.find(p => p.id === product.id)) {
            addBotMessage(`<b>${escHtml(product.name)}</b> đã có trong danh sách so sánh rồi.`);
            return;
        }
        if (compareQueue.length >= 4) {
            addBotMessage('Danh sách so sánh tối đa <b>4 sản phẩm</b>. Xoá bớt để thêm mới.');
            return;
        }
        compareQueue.push({ id: product.id, name: product.name, imageUrl: product.imageUrl });
        renderPending();
        addBotMessage(`Đã thêm <b>${escHtml(product.name)}</b> vào danh sách so sánh.${compareQueue.length >= 2 ? ' Nhấn <b>So sánh ngay</b> khi sẵn sàng!' : ' Thêm ít nhất 1 sản phẩm nữa.'}`);
    }

    window.techChatbot = {
        addById: async function (id) {
            const found = searchResultCache.find(p => p.id === id);
            if (found) { addToQueue(found); return; }
            // Nếu không có trong cache, gọi API detail
            try {
                const res = await fetch(`${API_BASE}/api/products/${id}`);
                if (res.ok) {
                    const p = await res.json();
                    addToQueue({ id: p.id, name: p.name, imageUrl: p.imageUrl });
                }
            } catch { /* ignore */ }
        },
        removeFromQueue: function (id) {
            compareQueue = compareQueue.filter(p => p.id !== id);
            renderPending();
        },
        compareNow: async function () {
            showTyping();
            scrollToBottom();
            chatWindow.classList.add('open');
            await doCompare();
        },
        clearQueue: function () {
            compareQueue = [];
            renderPending();
        }
    };

    function renderPending() {
        if (!pendingWrap) return;
        if (compareQueue.length === 0) {
            pendingWrap.innerHTML = '';
            return;
        }
        const tags = compareQueue.map(p =>
            `<span>${escHtml(p.name)} <button onclick="window.techChatbot.removeFromQueue(${p.id})" title="Xoá">×</button></span>`
        ).join('');
        pendingWrap.innerHTML = `
            <div class="compare-pending">
                <strong>Đang so sánh (${compareQueue.length}/4):</strong>
                <div class="pending-list">${tags}</div>
                <div class="pending-actions">
                    <button class="btn-compare-now" onclick="window.techChatbot.compareNow()" ${compareQueue.length < 2 ? 'disabled' : ''}>
                        <i class="fa fa-balance-scale me-1"></i>So sánh ngay
                    </button>
                    <button class="btn-clear" onclick="window.techChatbot.clearQueue()">Xoá tất cả</button>
                </div>
            </div>`;
    }

    // ─── DOM helpers ───────────────────────────────────────────────────────────
    function addUserMessage(text) {
        const row = document.createElement('div');
        row.className = 'msg-row user';
        row.innerHTML = `<div class="msg-bubble">${escHtml(text)}</div>`;
        messages.appendChild(row);
        scrollToBottom();
    }

    function addBotMessage(html) {
        removeTyping();
        const row = document.createElement('div');
        row.className = 'msg-row bot';
        row.innerHTML = `
            <div class="msg-avatar"><i class="fa fa-robot"></i></div>
            <div class="msg-bubble">${html}</div>`;
        messages.appendChild(row);
        scrollToBottom();
    }

    function showTyping() {
        removeTyping();
        const row = document.createElement('div');
        row.className = 'msg-row bot typing-row';
        row.innerHTML = `
            <div class="msg-avatar"><i class="fa fa-robot"></i></div>
            <div class="msg-bubble"><span class="typing-dots"><span></span><span></span><span></span></span></div>`;
        messages.appendChild(row);
        scrollToBottom();
    }

    function removeTyping() {
        document.querySelectorAll('.typing-row').forEach(el => el.remove());
    }

    function scrollToBottom() {
        messages.scrollTop = messages.scrollHeight;
    }

    // ─── API helpers ───────────────────────────────────────────────────────────
    async function fetchSearch(keyword) {
        try {
            const res = await fetch(`${API_BASE}/api/products/search?q=${encodeURIComponent(keyword)}`);
            if (!res.ok) return [];
            return await res.json();
        } catch {
            return [];
        }
    }

    // ─── Utils ─────────────────────────────────────────────────────────────────
    function escHtml(str) {
        if (!str) return '';
        return String(str)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function formatPrice(min, max) {
        const fmt = v => v != null
            ? new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 }).format(v)
            : null;
        const fMin = fmt(min);
        const fMax = fmt(max);
        if (fMin && fMax && min !== max) return `${fMin} – ${fMax}`;
        return fMin || fMax || 'Liên hệ';
    }
})();
