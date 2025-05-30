﻿let container = document.getElementById('container');

toggle = () => {
    container.classList.toggle('sign-in');
    container.classList.toggle('sign-up');

    // إعادة توجيه بناءً على الحالة
    if (container.classList.contains('sign-up')) {
        window.location.href = '/Account/SignUp';
    } else {
        window.location.href = '/Account/Login';
    }
};

// تحديد الحالة الافتراضية بناءً على عنوان URL مع تنشيط الـ Animation
document.addEventListener('DOMContentLoaded', () => {
    const path = window.location.pathname.toLowerCase();
    if (path.includes('signup')) {
        container.classList.add('sign-up');
        container.classList.remove('sign-in');
    } else if (path.includes('login')) {
        container.classList.add('sign-in');
        container.classList.remove('sign-up');
    } else {
        container.classList.add('sign-in');
        container.classList.remove('sign-up');
    }
    // إضافة تأخير صغير لضمان تفعيل الـ Animation
    setTimeout(() => {
        container.classList.add('active');
    }, 100);
    console.log('Current state:', container.classList);
});