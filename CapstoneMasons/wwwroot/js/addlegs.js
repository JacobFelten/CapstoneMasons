function addFields(shapes) {
    var shapesInput ="";
    for (i = 0; i < shapes; i++) {
        shapesInput +=
            `<div class="md-form col-mb-1 form-group">
            <i class=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]]far fa-square	 prefix grey-text"></i>
            <input type="number" id="" class="form-control validate">
                <label data-error="wrong" data-success="right" for="form29">Shape ${i+1} legs</label>
         </div>`;
    }
        document.getElementById("shapes").innerHTML = shapesInput;
}