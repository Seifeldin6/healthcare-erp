import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class RegisterReviewSectionCard extends StatelessWidget {
  final String title;
  final VoidCallback onEdit;
  final Widget child;

  const RegisterReviewSectionCard({
    super.key,
    required this.title,
    required this.onEdit,
    required this.child,
  });

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Container(
      width: double.infinity,
      padding: EdgeInsets.all(AppDimensions.spacing16.w),
      decoration: BoxDecoration(
        color: AppColors.surface,
        borderRadius: BorderRadius.circular(AppDimensions.cardRadius.r),
        border: Border.all(color: AppColors.line),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                title,
                style: AppTextStyles.caption12MutedRegular.copyWith(
                  fontWeight: FontWeight.w600,
                ),
              ),
              InkWell(
                onTap: onEdit,
                borderRadius: BorderRadius.circular(8.r),
                child: Padding(
                  padding: EdgeInsets.symmetric(horizontal: 4.w, vertical: 2.h),
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(
                        Icons.edit_outlined,
                        color: AppColors.primary,
                        size: 14.sp,
                      ),
                      SizedBox(width: 4.w),
                      Text(
                        s.editButton,
                        style: AppTextStyles.body14PrimaryBold.copyWith(
                          fontSize: 13.sp,
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ],
          ),
          SizedBox(height: AppDimensions.spacing12.h),
          child,
        ],
      ),
    );
  }
}
