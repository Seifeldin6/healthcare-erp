import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

abstract final class AppTextStyles {
  static TextStyle display = TextStyle(
    fontSize: 30.sp,
    height: 38 / 30,
    fontWeight: FontWeight.w700,
  );

  static TextStyle title = TextStyle(
    fontSize: 26.sp,
    height: 32 / 26,
    fontWeight: FontWeight.w700,
  );

  static TextStyle heading = TextStyle(
    fontSize: 18.sp,
    height: 24 / 18,
    fontWeight: FontWeight.w600,
  );

  static TextStyle body = TextStyle(
    fontSize: 16.sp,
    height: 24 / 16,
    fontWeight: FontWeight.w400,
  );

  static TextStyle label = TextStyle(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeight.w500,
  );

  static TextStyle caption = TextStyle(
    fontSize: 13.sp,
    height: 19 / 13,
    fontWeight: FontWeight.w400,
  );
}
