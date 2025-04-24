document.addEventListener('DOMContentLoaded', function () {
    const hamburgerMenu = document.getElementById('hamburger-menu');
    const sidebar = document.getElementById('sidebar');

    hamburgerMenu.addEventListener('click', function () {
        sidebar.classList.toggle('open');
    });


    
        const forms = document.querySelectorAll('form[asp-action="AddToCart"]');

        forms.forEach(form => {
            form.addEventListener('submit', async function (e) {
                e.preventDefault();

                const formData = new FormData(this);
                const response = await fetch(this.action, {
                    method: 'POST',
                    body: formData
                });

                if (response.ok) {
                    const result = await response.json();
                    if (result.success) {
                        alert('Product added to cart successfully!');
                        // Optionally update cart count or total price
                    }
                } else {
                    alert('Failed to add product to cart.');
                }
            });
        });

    
        // Success Message Animation
        const successMessage = document.querySelector(".order-summary-container h2");
        if (successMessage) {
            successMessage.style.opacity = "0";
            successMessage.style.transition = "opacity 2s ease-in-out";
            setTimeout(() => {
                successMessage.style.opacity = "1";
            }, 500);
        }

        // Smooth Scroll for Back to Home Button
        const backButton = document.querySelector(".btn-back-home");
        if (backButton) {
            backButton.addEventListener("click", function (event) {
                event.preventDefault();
                window.scrollTo({ top: 0, behavior: "smooth" });
                setTimeout(() => {
                    window.location.href = "/";
                }, 500);
            });
        }
    
   

});
