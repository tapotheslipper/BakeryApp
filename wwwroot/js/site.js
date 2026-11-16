/* Cart in local storage */
const CART_KEY = 'cart';
const getCart = () => {
    try { return JSON.parse(localStorage.getItem(CART_KEY)) || []; }
    catch { return []; }
};
const saveCart = c => { localStorage.setItem(CART_KEY, JSON.stringify(c)); updateCount(); };
const cartTotal = c => c.reduce((s, i) => s + i.price * i.qty, 0);
const esc = s => String(s).replace(/[&<>"]/g, ch => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' }[ch]));

function updateCount() {
    const el = document.getElementById('cartCount');
    if (el) el.textContent = getCart().reduce((s, i) => s + i.qty, 0);
}

function addToCart(id, name, price) {
    const cart = getCart();
    const item = cart.find(i => i.id === id);
    if (item) {
        item.qty++
    } else {
        cart.push({ id, name, price, qty: 1 });
    }
    saveCart(cart);
}

function renderCart() {
    const box = document.getElementById('cartBody');
    if (!box) return;

    const cart = getCart();
    if (!cart.length) {
        box.innerHTML = '<p>Корзина пуста. <a href="/catalog">В каталог</a></p>';
        return;
    }

    let rows = cart.map(item => `
        <tr>
            <td>${esc(item.name)}</td>
            <td>${item.price}</td>
            <td>
                <button class="btn qty" data-act="minus" data-id="${item.id}">-</button>
                ${item.qty}
                <button class="btn qty" data-act="plus" data-id="${item.id}">+</button>
            </td>
            <td>${item.price * item.qty}</td>
            <td><button class="link-btn" data-act="del" data-id="${item.id}">✕</button></td>
        </tr>
    `).join('');

    box.innerHTML = `
        <div class="table-wrap">
            <table class="table">
                <thead>
                    <tr>
                        <th>Товар</th>
                        <th>Цена</th>
                        <th>Кол-во</th>
                        <th>Сумма</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    ${rows}
                </tbody>
            </table>
        </div>
        <p class="total">Итого: <b>${cartTotal(cart)} ₽</b></p>
        <a class="btn" href="/checkout">Оформить заказ</a>
    `;
}

function toggleMenu() { document.getElementById('navMenu').classList.toggle('active'); }

document.addEventListener('click', e => {
    const add = e.target.closest('.add-to-cart');
    if (add) {
        addToCart(+add.dataset.id, add.dataset.name, +add.dataset.price);
        setTimeout(() => add.textContent = 'В корзину', 900);
        return;
    }
    const act = e.target.closest('[data-act]');
    if (act) {
        const id = +act.dataset.id;
        let cart = getCart();
        if (act.dataset.act === 'plus') {
            const item = cart.find(i => i.id === id);
            item.qty++;
        }
        if (act.dataset.act === 'minus') item.qty--;
        if (act.dataset.act === 'del') item.qty = 0;
        saveCart(cart.filter(i => i.qty > 0));
        renderCart();
        return;
    }
    const more = e.target.closest('.more');
    if (more) {
        dlgName.textContent = more.dataset.name;
        dlgDesc.textContent = more.dataset.desc;
        dlgComp.textContent = more.dataset.comp;
        dlgCal.textContent = more.dataset.cal;
        productDialog.showModal();
    }
});

/* Initialize elements */
document.addEventListener('DOMContentLoaded', () => {
    updateCount();
    renderCart();

    // catalog search
    const search = document.getElementById('search');
    if (search) search.addEventListener('input', () => {
        const q = search.value.trim().toLowerCase();
        document.querySelectorAll('.product-card').forEach(c => c.style.display = c.dataset.name.includes(q) ? '' : 'none');
    });

    // dialog window
    const dlg = document.getElementById('productDialog');
    if (dlg) {
        document.getElementById('dlgClose').onclick = () => dlg.close();
        dlg.addEventListener('click', e => { if (e.target === dlg) dlg.close(); });
    }

    // checkout
    const form = document.getElementById('checkoutForm');
    if (form) {
        document.getElementById('checkoutTotal').textContent = cartTotal(getCart());
        form.addEventListener('submit', () => {
            document.getElementById('CartJson').value = JSON.stringify(getCart().map(i => ({ id: i.id, qty: i.qty })));
        });
    }

    // slider
    const slides = document.querySelectorAll('.slide');
    if (slides.length) {
        let n = 0;
        const show = k => {
            n = (k + slides.length) % slides.length;
            slides.forEach((s, i) => s.classList.toggle('active', i == n));
        };
        document.querySelector('.slider-btn.prev').onclick = () => show(n - 1);
        document.querySelector('.slider-btn.next').onclick = () => show(n + 1);
        setInterval(() => show(n + 1), 5000);
    }
});