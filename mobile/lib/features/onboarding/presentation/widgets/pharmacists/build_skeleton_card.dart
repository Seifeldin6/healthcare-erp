import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class BuildSkeletonCard extends StatelessWidget {
  const BuildSkeletonCard({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);
    return Container(
      padding: EdgeInsetsDirectional.all(AppDimensions.spacing12.w),
      decoration: BoxDecoration(
        color: Colors.white.withValues(alpha: 0.7),
        borderRadius: BorderRadius.vertical(top: Radius.circular(18.r)),
      ),
      child: Row(
        children: [
          Container(
            width: 44.w,
            height: 44.w,
            decoration: const BoxDecoration(
              color: AppColors.skeletonBg,
              shape: BoxShape.circle,
            ),
            alignment: Alignment.center,
            child: Text(
              s.pharmacistMockupSkeletonInitials,
              style: AppTextStyles.label14BodyMedium,
            ),
          ),
          SizedBox(width: AppDimensions.spacing12.w),
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Container(
                width: 110.w,
                height: 8.h,
                decoration: BoxDecoration(
                  color: AppColors.skeletonLine,
                  borderRadius: BorderRadius.circular(4.r),
                ),
              ),
              SizedBox(height: AppDimensions.spacing6.h),
              Container(
                width: 75.w,
                height: 8.h,
                decoration: BoxDecoration(
                  color: AppColors.skeletonLine,
                  borderRadius: BorderRadius.circular(4.r),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
