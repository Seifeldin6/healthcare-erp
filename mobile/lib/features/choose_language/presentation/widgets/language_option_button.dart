import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';

class LanguageOptionButton extends StatelessWidget {
  const LanguageOptionButton({
    super.key,
    required this.label,
    required this.onTap,
  });

  final String label;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return Material(
      color: AppColors.languageOptionBg,
      borderRadius: BorderRadius.circular(AppDimensions.buttonRadius.r),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(AppDimensions.buttonRadius.r),
        splashColor: AppColors.primaryTint,
        highlightColor: AppColors.primaryTint.withValues(alpha: 0.5),
        child: Container(
          width: double.infinity,
          height: 60.h,
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(AppDimensions.buttonRadius.r),
            border: Border.all(color: AppColors.line, width: 1),
          ),
          alignment: Alignment.center,
          child: Text(
            label,
            style: AppTextStyles.heading18InkSemiBold,
          ),
        ),
      ),
    );
  }
}