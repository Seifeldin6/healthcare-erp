import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';

class AppPrimaryButton extends StatelessWidget {
  final String text;
  final VoidCallback? onPressed;
  final IconData? suffixIcon;
  final IconData? prefixIcon;
  final bool isLoading;

  const AppPrimaryButton({
    super.key,
    required this.text,
    required this.onPressed,
    this.suffixIcon,
    this.prefixIcon,
    this.isLoading = false,
  });

  @override
  Widget build(BuildContext context) {
    return ElevatedButton(
      onPressed: isLoading ? null : onPressed,
      child: isLoading
          ? SizedBox(
              width: 22.w,
              height: 22.w,
              child: const CircularProgressIndicator(
                strokeWidth: 2.5,
                color: Colors.white,
              ),
            )
          : Row(
              mainAxisAlignment: MainAxisAlignment.center,
              mainAxisSize: MainAxisSize.min,
              children: [
                if (prefixIcon != null) ...[
                  Icon(prefixIcon, size: 18.sp, color: Colors.white),
                  SizedBox(width: AppDimensions.spacing8.w),
                ],
                Text(text, style: AppTextStyles.body20WhiteSemiBold),
                if (suffixIcon != null) ...[
                  SizedBox(width: AppDimensions.spacing8.w),
                  Icon(suffixIcon, size: 18.sp, color: Colors.white),
                ],
              ],
            ),
    );
  }
}
