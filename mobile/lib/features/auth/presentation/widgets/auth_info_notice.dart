import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';

class AuthInfoNotice extends StatelessWidget {
  const AuthInfoNotice({super.key, required this.message});
  final String message;
  @override
  Widget build(BuildContext context) {
    return Container(
      width: double.infinity,
      padding: EdgeInsets.symmetric(
        horizontal: AppDimensions.spacing16.w,
        vertical: AppDimensions.spacing12.h,
      ),
      decoration: BoxDecoration(
        color: AppColors.noticeBg,
        borderRadius: BorderRadius.circular(AppDimensions.cardRadius.r),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(Icons.info_outline_rounded, color: AppColors.muted, size: 18.sp),
          SizedBox(width: AppDimensions.spacing12.w),
          Expanded(
            child: Text(
              message,
              //   s.patientPortalNotice,
              style: AppTextStyles.caption12MutedRegular.copyWith(
                height: 18 / 12,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
