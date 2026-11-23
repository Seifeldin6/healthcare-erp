import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/helpers/extensions.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class LoginErrorBanner extends StatelessWidget {
  const LoginErrorBanner({super.key, this.title, this.message});

  final String? title;
  final String? message;

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);
    final displayTitle = (!(title.isNullOrEmpty()))
        ? title!
        : s.loginErrorTitle;
    final displayMessage = (!(message.isNullOrEmpty()))
        ? message!
        : s.loginErrorDescription;

    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(AppDimensions.spacing16.w),
      decoration: BoxDecoration(
        color: AppColors.errorTint,
        borderRadius: BorderRadius.circular(AppDimensions.cardRadius.r),
        border: Border.all(color: AppColors.errorBorder),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(
            Icons.error_outline_rounded,
            color: AppColors.error,
            size: 20.sp,
          ),
          SizedBox(width: AppDimensions.spacing12.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  displayTitle,
                  style: AppTextStyles.label14InkBold.copyWith(
                    color: AppColors.error,
                  ),
                ),
                SizedBox(height: AppDimensions.spacing4.h),
                Text(
                  displayMessage,
                  style: AppTextStyles.caption13MutedRegular.copyWith(
                    color: AppColors.error,
                    height: 18 / 13,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
