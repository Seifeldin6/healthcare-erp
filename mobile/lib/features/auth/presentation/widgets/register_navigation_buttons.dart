import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/core/widgets/app_primary_button.dart';
import 'package:healthcare_app/generated/l10n.dart';

class RegisterNavigationButtons extends StatelessWidget {
  final VoidCallback onBack;
  final VoidCallback onContinue;
  final String? continueText;
  final IconData? continueSuffixIcon;
  final bool isLoading;

  const RegisterNavigationButtons({
    super.key,
    required this.onBack,
    required this.onContinue,
    this.continueText,
    this.continueSuffixIcon = Icons.arrow_forward_rounded,
    this.isLoading = false,
  });

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Row(
      children: [
        Expanded(
          flex: 1,
          child: AppPrimaryButton(
            text: s.backButton,
            backgroundColor: AppColors.surface,
            textStyle: AppTextStyles.label14InkSemiBold.copyWith(
              fontSize: 16.sp,
            ),
            borderSide: const BorderSide(color: AppColors.line),
            onPressed: isLoading ? null : onBack,
          ),
        ),
        SizedBox(width: AppDimensions.spacing12.w),
        Expanded(
          flex: 2,
          child: AppPrimaryButton(
            text: continueText ?? s.continueButton,
            suffixIcon: continueSuffixIcon,
            isLoading: isLoading,
            onPressed: onContinue,
          ),
        ),
      ],
    );
  }
}
