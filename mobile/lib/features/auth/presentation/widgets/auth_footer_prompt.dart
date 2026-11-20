import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';

class AuthFooterPrompt extends StatelessWidget {
  const AuthFooterPrompt({
    super.key,
    required this.leadingText,
    required this.actionText,
    required this.onActionTap,
  });

  final String leadingText;
  final String actionText;
  final VoidCallback onActionTap;

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Text(
          leadingText,
          style: AppTextStyles.label15BodyRegular,
        ),
        SizedBox(width: 4.w),
        GestureDetector(
          onTap: onActionTap,
          behavior: HitTestBehavior.opaque,
          child: Padding(
            padding: EdgeInsets.symmetric(vertical: 4.h),
            child: Text(
              actionText,
              style: AppTextStyles.label15PrimaryBold,
            ),
          ),
        ),
      ],
    );
  }
}