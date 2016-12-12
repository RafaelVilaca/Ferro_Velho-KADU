$(document).ready(function () {

    $(".data").mask("99/99/9999");

    $(".telefone").mask("(99)99999-9999");
    
    $('.decimal').priceFormat({
        prefix: '',
        centsLimit: 2,
        thousandsSeparator: '',
        centsSeparator: ','
    });

    $(".CPF").mask("999.999.999-99");

    //$(".peso").mask("999.999,99");

    //$(".money").mask("999.999,99");

    $(".int").mask("9999999");

    $(".money").maskMoney({ showSymbol: true, symbol: "R$", decimal: ",", thousands: "." });
    $(".peso").maskPeso({ showSymbol: true, symbol: "KG", decimal: ".", thousands: "." });
});