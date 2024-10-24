const $card = $(".card");
const $category = $("#categoryId");

$category.on("change",function () {
    this.form.submit();
});

$card.on('mouseenter', function () {
    const $closestCard = $(this).closest('.card'); // cache the card element
    const $buttonCart = $closestCard.find('#AddCart');
    const isAvailable = $closestCard.find('#IsAvailable').val() === "True";

    if (!isAvailable) {
        $buttonCart.val("Stock Out");
        $buttonCart.css('background-color', 'red');
        $buttonCart.prop('disabled', true);
    } else {
        $buttonCart.prop('disabled', false);
    }
});

$card.on('mouseleave', function () {
    const $closestCard = $(this).closest('.card'); // cache the card element
    const $buttonCart = $closestCard.find('#AddCart');

    $buttonCart.val("Add To Cart");
    $buttonCart.css('background-color', 'green');
    $buttonCart.prop('disabled', false);
});




