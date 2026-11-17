import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'app_colors.dart';
import 'font_weight_helper.dart';

abstract final class AppTextStyles {
  // Base styles
  static TextStyle display = TextStyle(
    fontSize: 30.sp,
    height: 38 / 30,
    fontWeight: FontWeightHelper.bold,
  );

  static TextStyle title = TextStyle(
    fontSize: 26.sp,
    height: 32 / 26,
    fontWeight: FontWeightHelper.bold,
  );

  static TextStyle heading = TextStyle(
    fontSize: 18.sp,
    height: 24 / 18,
    fontWeight: FontWeightHelper.semiBold,
  );

  static TextStyle body = TextStyle(
    fontSize: 16.sp,
    height: 24 / 16,
    fontWeight: FontWeightHelper.regular,
  );

  static TextStyle label = TextStyle(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.medium,
  );

  static TextStyle caption = TextStyle(
    fontSize: 13.sp,
    height: 19 / 13,
    fontWeight: FontWeightHelper.regular,
  );

  // Display & Titles
  static TextStyle display30InkBold = display.copyWith(color: AppColors.ink);
  static TextStyle title26InkBold = title.copyWith(color: AppColors.ink);
  static TextStyle heading18InkSemiBold = heading.copyWith(
    color: AppColors.ink,
  );

  // Body (16.sp & 20.sp)
  static TextStyle body20WhiteSemiBold = TextStyle(
    fontSize: 20.sp,
    height: 24 / 20,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.surface,
  );

  static TextStyle body16BodyRegular = body.copyWith(color: AppColors.body);
  static TextStyle body16WhiteRegular = body.copyWith(color: AppColors.surface);
  static TextStyle body16InkRegular = body.copyWith(color: AppColors.ink);

  static TextStyle body16WhiteSemiBold = TextStyle(
    fontSize: 16.sp,
    height: 24 / 16,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.surface,
  );

  static TextStyle body16PrimaryBold = TextStyle(
    fontSize: 16.sp,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.primary,
  );

  static TextStyle body16WarmInkBold = TextStyle(
    fontSize: 16.sp,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.warmInk,
  );

  // Label (15.sp & 14.sp)
  static TextStyle label15InkBold = TextStyle(
    fontSize: 15.sp,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.ink,
  );

  static TextStyle label14BodyMedium = label.copyWith(color: AppColors.body);

  static TextStyle label14InkBold = TextStyle(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.ink,
  );

  static TextStyle label14WhiteBold = TextStyle(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.surface,
  );

  static TextStyle label14PrimaryBold = TextStyle(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.primary,
  );

  static TextStyle label14WarmInkBold = TextStyle(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.warmInk,
  );

  static TextStyle label14InkSemiBold = TextStyle(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.ink,
  );

  static TextStyle label14PrimarySemiBold = TextStyle(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.primary,
  );

  static TextStyle label14MutedRegular = TextStyle(
    fontSize: 14.sp,
    height: 20 / 14,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.muted,
  );

  // Caption (13.sp, 12.sp, 11.sp, 10.sp)
  static TextStyle caption13BodyRegular = caption.copyWith(
    color: AppColors.body,
  );

  static TextStyle caption13WhiteRegular = TextStyle(
    fontSize: 13.sp,
    height: 18 / 13,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.surface,
  );

  static TextStyle caption13InkRegular = TextStyle(
    fontSize: 13.sp,
    height: 18 / 13,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.ink,
  );

  static TextStyle caption13InkSemiBold = TextStyle(
    fontSize: 13.sp,
    height: 19 / 13,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.ink,
  );

  static TextStyle caption13WhiteSemiBold = TextStyle(
    fontSize: 13.sp,
    height: 19 / 13,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.surface,
  );

  static TextStyle caption12MutedSemiBold = TextStyle(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.muted,
  );

  static TextStyle caption12SuccessSemiBold = TextStyle(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.success,
  );

  static TextStyle caption13InkMedium = TextStyle(
    fontSize: 13.sp,
    fontWeight: FontWeightHelper.medium,
    color: AppColors.ink,
  );

  static TextStyle caption12MutedRegular = TextStyle(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.muted,
  );

  static TextStyle caption12MutedBoldUppercase = TextStyle(
    fontSize: 12.sp,
    letterSpacing: 0.6,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.muted,
  );

  static TextStyle caption12AllergySemiBold = TextStyle(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.allergyText,
  );

  static TextStyle caption12ConditionSemiBold = TextStyle(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.conditionText,
  );

  static TextStyle caption12PrimarySemiBold = TextStyle(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.semiBold,
    color: AppColors.primary,
  );

  static TextStyle caption11SuccessMedium = TextStyle(
    fontSize: 11.sp,
    fontWeight: FontWeightHelper.medium,
    color: AppColors.success,
  );

  static TextStyle caption11MutedMedium = TextStyle(
    fontSize: 11.sp,
    fontWeight: FontWeightHelper.medium,
    color: AppColors.muted,
  );

  static TextStyle caption11BodyRegular = TextStyle(
    fontSize: 11.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.body,
  );

  static TextStyle caption11MutedRegular = TextStyle(
    fontSize: 11.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.muted,
  );

  static TextStyle caption11WhiteMutedRegular = TextStyle(
    fontSize: 11.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.indicatorInactive,
  );

  static TextStyle caption10BodyRegular = TextStyle(
    fontSize: 10.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.body,
  );

  static TextStyle caption12PrimaryBold = TextStyle(
    fontSize: 12.sp,
    fontWeight: FontWeightHelper.bold,
    color: AppColors.primary,
  );

  static TextStyle caption13MutedRegular = TextStyle(
    fontSize: 13.sp,
    fontWeight: FontWeightHelper.regular,
    color: AppColors.muted,
  );
}
