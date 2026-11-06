/* Trang Thông tin cá nhân của HLV (/PT/Profile).
   - Chọn ảnh: xem trước ngay trong thẻ hồ sơ, kiểm tra loại file và dung lượng trước khi gửi.
   - Gõ họ tên / chuyên môn: thẻ hồ sơ cập nhật theo.
   - Đếm ký tự phần giới thiệu; hỏi lại trước khi xoá ảnh. */
(function () {
  "use strict";

  var form = document.getElementById("cxProfileForm");
  if (!form) return;

  var MAX_BYTES = 5 * 1024 * 1024;
  var TYPES = ["image/jpeg", "image/png", "image/webp"];

  var fileInput = document.getElementById("avatarFile");
  var photo = form.querySelector("[data-photo]");
  var undoBtn = document.getElementById("cxAvatarUndo");
  var deleteBtn = document.getElementById("cxAvatarDelete");
  var note = document.getElementById("cxAvatarNote");
  var noteDefault = note ? note.textContent : "";
  var originalPhoto = photo ? photo.innerHTML : "";
  var objectUrl = null;

  function initials(name) {
    var w = (name || "").trim().split(/\s+/).filter(Boolean);
    if (!w.length) return "?";
    return (w.length === 1 ? w[0][0] : w[w.length - 2][0] + w[w.length - 1][0]).toUpperCase();
  }

  function setNote(msg, isError) {
    if (!note) return;
    note.textContent = msg || noteDefault;
    note.classList.toggle("is-error", !!isError);
  }

  function resetPhoto() {
    if (objectUrl) { URL.revokeObjectURL(objectUrl); objectUrl = null; }
    fileInput.value = "";
    photo.innerHTML = originalPhoto;
    photo.classList.remove("is-new");
    if (undoBtn) undoBtn.hidden = true;
    if (deleteBtn) deleteBtn.hidden = false;
    setNote("");
  }

  if (fileInput && photo) {
    fileInput.addEventListener("change", function () {
      var file = fileInput.files && fileInput.files[0];
      if (!file) { resetPhoto(); return; }
      if (TYPES.indexOf(file.type) === -1) {
        resetPhoto();
        setNote("Chỉ nhận ảnh JPG, PNG hoặc WEBP.", true);
        return;
      }
      if (file.size > MAX_BYTES) {
        resetPhoto();
        setNote("Ảnh này nặng " + (file.size / 1048576).toFixed(1) + " MB, tối đa 5 MB. Hãy chọn ảnh nhỏ hơn.", true);
        return;
      }
      if (objectUrl) URL.revokeObjectURL(objectUrl);
      objectUrl = URL.createObjectURL(file);
      var keepCam = photo.querySelector(".cx-pcard-cam");
      photo.innerHTML = "";
      var img = document.createElement("img");
      img.alt = "Ảnh đại diện mới";
      img.src = objectUrl;
      photo.appendChild(img);
      if (keepCam) photo.appendChild(keepCam);
      photo.classList.add("is-new");
      if (undoBtn) undoBtn.hidden = false;
      if (deleteBtn) deleteBtn.hidden = true;
      setNote("Ảnh mới sẽ được lưu khi bạn bấm Lưu thay đổi.");
    });
  }
  if (undoBtn) undoBtn.addEventListener("click", resetPhoto);

  if (deleteBtn) {
    deleteBtn.addEventListener("click", function (e) {
      if (!window.confirm("Xoá ảnh đại diện? Hồ sơ sẽ hiện chữ viết tắt tên của bạn.")) e.preventDefault();
    });
  }

  /* xem trước tên, chuyên môn; đếm ký tự giới thiệu */
  var nameInput = form.elements["FullName"];
  var specInput = form.elements["Specialization"];
  var bioInput = form.elements["Bio"];
  var pName = form.querySelector("[data-preview-name]");
  var pSpec = form.querySelector("[data-preview-spec]");
  var bioCount = form.querySelector("[data-bio-count]");

  if (nameInput && pName) {
    nameInput.addEventListener("input", function () {
      pName.textContent = nameInput.value.trim() || "Họ và tên";
      var ini = photo && photo.querySelector("[data-photo-initials]");
      if (ini) ini.textContent = initials(nameInput.value);
    });
  }
  if (specInput && pSpec) {
    specInput.addEventListener("input", function () { pSpec.textContent = specInput.value.trim() || "Chuyên môn"; });
  }
  if (bioInput && bioCount) {
    bioInput.addEventListener("input", function () { bioCount.textContent = bioInput.value.length; });
  }

  /* kiểm tra nhanh trước khi gửi (server vẫn kiểm tra lại) */
  form.addEventListener("submit", function (e) {
    var bad = null;
    [["FullName", "Vui lòng nhập họ và tên"], ["Specialization", "Vui lòng nhập chuyên môn"]].forEach(function (r) {
      var input = form.elements[r[0]];
      var err = form.querySelector('[data-valmsg-for="' + r[0] + '"]');
      var empty = !input.value.trim();
      if (err) err.textContent = empty ? r[1] : "";
      input.closest(".cx-field").classList.toggle("is-invalid", empty);
      if (empty && !bad) bad = input;
    });
    if (bad) { e.preventDefault(); bad.focus(); return; }
    var btn = form.querySelector('button[type="submit"]:not([form])');
    if (btn) { btn.disabled = true; btn.textContent = "Đang lưu…"; }
  });
})();
