import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'app_colors.dart';
import 'font_weight_helper.dart';

abstract final class AppTextStyles {
  static const String _primaryFont = 'PlusJakartaSans';
  static const List<String> _fallbackFonts = ['IBMPlexSansArabic'];

  static TextStyle _base({
    required double fontSize,
    required FontWeight fontWeight,
    double? height,
    double? letterSpacing,
    Color? color,
  }) {
    return TextStyle(
      fontFamily: _primaryFont,
      fontFamilyFallback: _fallbackFonts,
      fontSize: fontSize,
      fontWeight: fontWeight,
      height: height,
      letterSpacing: letterSpacing,
      color: color,
    );
  }

  // Base styles
  static TextStyle display = _base(
    fontSize: 30.sp,
    height: 38 / 30,
    fontWeight: FontWeightHelper.bold,
  );

  static TextStyle title = _base(
    fontSize: 26.sp,
    height: 32 / 26,
    fontWeight: FontWeightHelper.bold,
  );

  static TextStyle heading = _base(
    fontSize: 18.sp,
    height: 24 / 18,
    fontWeight: FontWeightHelper.semiBold,
  );

  static TextStyle body = _base(
    fontSize: 16.sp,
    height: 24 / 16,
    fontWeight: FontWeightHelper.regular,
  );

  static TextStyle label = _base(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.medium,
  );

  static TextStyle label15BodyRegular = _base(
    fontSize: 15.sp,
    height: 1.0,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.body,
  );

  static TextStyle label15PrimaryBold = _base(
    fontSize: 15.sp,
    height: 1.0,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.primary,
  );

  static TextStyle caption = _base(
    fontSize: 13.sp,
    height: 19 / 13,
    fontWeight: FontWeightHelper.regular,
  );

  // Display & Titles
  static TextStyle display30InkBold = display.copyWith(color: AppColors.ink);
  static TextStyle display30InkExtraBold = _base(
    fontSize: 30.sp,
    height: 38 / 30,
    letterSpacing: -0.6,
    fontWeight: FontWeightHelper.extraBold,
    color: AppColors.ink,
  );
  static TextStyle title26InkBold = title.copyWith(color: AppColors.ink);
  static TextStyle title24InkBold = _base(
    fontSize: 24.sp,
    height: 34 / 24,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.ink,
  );

  static TextStyle heading18InkSemiBold = heading.copyWith(
    color: AppColors.ink,
  );

  static TextStyle heading20InkExtraBold = _base(
    fontSize: 20.sp,
    height: 1.0,
    letterSpacing: -0.3,
    fontWeight: FontWeightHelper.extraBold,
    color: AppColors.ink,
  );

  // Body (16.sp & 20.sp)
  static TextStyle body20WhiteSemiBold = _base(
    fontSize: 20.sp,
    height: 24 / 20,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.surface,
  );

  static TextStyle body16BodyRegular = body.copyWith(color: AppColors.body);
  static TextStyle body16WhiteRegular = body.copyWith(color: AppColors.surface);
  static TextStyle body16InkRegular = body.copyWith(color: AppColors.ink);

  static TextStyle body16WhiteSemiBold = _base(
    fontSize: 16.sp,
    height: 24 / 16,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.surface,
  );

  static TextStyle body16PrimaryBold = _base(
    fontSize: 16.sp,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.primary,
  );

  static TextStyle body16WarmInkBold = _base(
    fontSize: 16.sp,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.warmInk,
  );

  static TextStyle body16MutedSemiBold = _base(
    fontSize: 16.sp,
    height: 22 / 16,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.muted,
  );

  // Label (15.sp & 14.sp)
  static TextStyle label15InkBold = _base(
    fontSize: 15.sp,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.ink,
  );

  static TextStyle label14BodyMedium = label.copyWith(color: AppColors.body);

  static TextStyle label14InkBold = _base(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.ink,
  );

  static TextStyle label14WhiteBold = _base(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.surface,
  );

  static TextStyle label14PrimaryBold = _base(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.primary,
  );

  static TextStyle label14WarmInkBold = _base(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.warmInk,
  );

  static TextStyle label14InkSemiBold = _base(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.ink,
  );

  static TextStyle label14PrimarySemiBold = _base(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.primary,
  );

  static TextStyle label14MutedRegular = _base(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.muted,
  );

  // Caption (13.sp, 12.sp, 11.sp, 10.sp)
  static TextStyle caption13BodyRegular = caption.copyWith(
    color: AppColors.body,
  );

  static TextStyle caption13WhiteRegular = _base(
    fontSize: 13.sp,
    height: 18 / 13,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.surface,
  );

  static TextStyle caption13InkRegular = _base(
    fontSize: 13.sp,
    height: 18 / 13,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.ink,
  );

  static TextStyle caption13InkSemiBold = _base(
    fontSize: 13.sp,
    height: 19 / 13,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.ink,
  );

  static TextStyle caption13WhiteSemiBold = _base(
    fontSize: 13.sp,
    height: 19 / 13,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.surface,
  );

  static TextStyle caption12MutedSemiBold = _base(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.muted,
  );

  static TextStyle caption12SuccessSemiBold = _base(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.success,
  );

  static TextStyle caption13InkMedium = _base(
    fontSize: 13.sp,
    fontWeight: FontWeightHelper.medium,
    color: AppColors.ink,
  );

  static TextStyle caption12MutedRegular = _base(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.muted,
  );

  static TextStyle caption12MutedBoldUppercase = _base(
    fontSize: 12.sp,
    letterSpacing: 0.6,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.muted,
  );

  static TextStyle caption12AllergySemiBold = _base(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.allergyText,
  );

  static TextStyle caption12ConditionSemiBold = _base(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.conditionText,
  );

  static TextStyle caption12PrimarySemiBold = _base(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.primary,
  );

  static TextStyle caption11SuccessMedium = _base(
    fontSize: 11.sp,
    fontWeight: FontWeightHelper.medium,
    color: AppColors.success,
  );

  static TextStyle caption11MutedMedium = _base(
    fontSize: 11.sp,
    fontWeight: FontWeightHelper.medium,
    color: AppColors.muted,
  );

  static TextStyle caption11BodyRegular = _base(
    fontSize: 11.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.body,
  );

  static TextStyle caption11MutedRegular = _base(
    fontSize: 11.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.muted,
  );

  static TextStyle caption11WhiteMutedRegular = _base(
    fontSize: 11.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.indicatorInactive,
  );

  static TextStyle caption10BodyRegular = _base(
    fontSize: 10.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.body,
  );

  static TextStyle caption12PrimaryBold = _base(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.primary,
  );

  static TextStyle caption13MutedRegular = _base(
    fontSize: 13.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.muted,
  );
}
