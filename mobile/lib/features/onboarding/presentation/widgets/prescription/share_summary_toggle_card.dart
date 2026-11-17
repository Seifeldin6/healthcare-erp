import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class ShareSummaryToggleCard extends StatelessWidget {
  const ShareSummaryToggleCard({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Container(
      padding: EdgeInsetsDirectional.symmetric(
        horizontal: 14.w,
        vertical: 12.h,
      ),
      decoration: BoxDecoration(
        color: AppColors.surface,
        borderRadius: BorderRadius.circular(22.r),
      ),
      child: Row(
        children: [
          Icon(
            Icons.verified_user_outlined,
            size: 20.sp,
            color: AppColors.primary,
          ),
          SizedBox(width: 10.w),
          Expanded(
            child: Text(
              s.prescriptionMockupShareSummary,
              style: AppTextStyles.caption13InkSemiBold,
            ),
          ),
          SizedBox(width: AppDimensions.spacing8.w),
          Container(
            width: 44.w,
            height: 26.h,
            padding: EdgeInsetsDirectional.all(3.w),
            decoration: BoxDecoration(
              color: AppColors.primary,
              borderRadius: BorderRadius.circular(13.r),
            ),
            alignment: AlignmentDirectional.centerEnd,
            child: Container(
              width: 20.w,
              height: 20.w,
              decoration: const BoxDecoration(
                color: AppColors.surface,
                shape: BoxShape.circle,
              ),
            ),
          ),
        ],
      ),
    );
  }
}