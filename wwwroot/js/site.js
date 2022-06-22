function updateTextView(_obj) {
    var num = getNumber(_obj.val());
    if (num == 0) {
        _obj.val('');
    } else {
        _obj.val(num.toLocaleString());
    }
}

function getNumber(_str) {
    var arr = _str.split('');
    var out = new Array();
    for (var cnt = 0; cnt < arr.length; cnt++) {
        if (isNaN(arr[cnt]) == false) {
            out.push(arr[cnt]);
        }
    }
    return Number(out.join(''));
}

$(document).ready(function () {
    $('.price').on('keyup', function () {
        updateTextView($(this));
    });
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#preview')
                .attr('src', e.target.result)
                .width(600)
                .height(600);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

let option = $(".type-select option:selected").val();
if (option.length == 1) {
    changeInput($(".type-select"))
}

function changeInput(input) {
    let stat = true;
    if (input.value === '1') {
        stat = false;
    }
    $("input[name='spec.Motherboard']").attr("disabled", stat);
    $("input[name='spec.MotherboardDetail']").attr("disabled", stat);
    $("input[name='spec.PowerSupply']").attr("disabled", stat);
    $("input[name='spec.Weight']").attr("disabled", !stat);
}