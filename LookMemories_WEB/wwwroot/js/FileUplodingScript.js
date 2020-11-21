function readURL1(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                console.log("ON load Clled");
                console.log(e);
                $('#UserImage').attr('src', e.target.result);
            }

            //for (var i = 0; i > input.files.length; i++) {
            //    reader.readAsDataURL(input.files[i]);

            //}

            reader.readAsDataURL(input.files[0]);
        }
    }