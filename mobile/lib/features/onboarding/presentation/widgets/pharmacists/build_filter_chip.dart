import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';

class BuildFilterChip extends StatelessWidget {
  final String label;
  final bool isSelected;

  const BuildFilterChip({
    super.key,
    required this.label,
    required this.isSelected,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsetsDirectional.symmetric(
        horizontal: AppDimensions.spacing16.w,
        vertical: AppDimensions.spacing6.h,
      ),
      decoration: BoxDecoration(
        color: isSelected ? AppColors.primary : Colors.white,
        borderRadius: BorderRadius.circular(20.r),
      ),
      child: Text(
        label,
        style: isSelected
            ? AppTextStyles.caption13WhiteSemiBold
            : AppTextStyles.caption13InkSemiBold,
      ),
    );
  }
}