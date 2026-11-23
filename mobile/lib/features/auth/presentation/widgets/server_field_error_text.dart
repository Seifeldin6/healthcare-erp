import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';

class ServerFieldErrorText extends StatelessWidget {
  final String errorMessage;

  const ServerFieldErrorText({super.key, required this.errorMessage});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.only(top: 6.h),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(
            Icons.error_outline_rounded,
            color: AppColors.error,
            size: 15.sp,
          ),
          SizedBox(width: 6.w),
          Expanded(
            child: Text(
              errorMessage,
              style: AppTextStyles.caption12MutedRegular.copyWith(
                color: AppColors.error,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
